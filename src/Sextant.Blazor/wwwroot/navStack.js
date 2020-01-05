var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
let notifyLocationChangedCallback = { assemblyName: "Sextant.Blazor", functionName: "NotifyLocationState" };
let testAnchor;
const SextantFunctions = {
    replaceState,
    getBaseUri: () => document.baseURI,
    getLocationHref: () => location.href,
    goBack,
    goToRoot
};
var SextantNavigationType;
(function (SextantNavigationType) {
    SextantNavigationType[SextantNavigationType["forward"] = 0] = "forward";
    SextantNavigationType[SextantNavigationType["back"] = 1] = "back";
    SextantNavigationType[SextantNavigationType["url"] = 2] = "url";
    SextantNavigationType[SextantNavigationType["popstate"] = 3] = "popstate";
})(SextantNavigationType || (SextantNavigationType = {}));
window.SextantFunctions = SextantFunctions;
// need this registered from the beginning!
// Will be triggered by user hitting back/forward button in browser.  Also triggered by history.go(), history.back(), etc  api calls.
window.addEventListener('popstate', (ev) => {
    console.log("Popstate event: " + ev.state);
    if (ev.state == null || !ev.state.shouldHandleInternally) {
        notifyNavigationAsync(SextantNavigationType.popstate, location.href, ev.state);
    }
});
document.addEventListener('click', event => {
    if (event.button !== 0 || eventHasSpecialKey(event)) {
        // Don't stop ctrl/meta-click (etc) from opening links in new tabs/windows
        return;
    }
    // Intercept clicks on all <a> elements where the href is within the <base href> URI space
    // We must explicitly check if it has an 'href' attribute, because if it doesn't, the result might be null or an empty string depending on the browser
    const anchorTarget = findClosestAncestor(event.target, 'A');
    const hrefAttributeName = 'href';
    if (anchorTarget && anchorTarget.hasAttribute(hrefAttributeName)) {
        const targetAttributeValue = anchorTarget.getAttribute('target');
        const opensInSameFrame = !targetAttributeValue || targetAttributeValue === '_self';
        if (!opensInSameFrame) {
            return;
        }
        const href = anchorTarget.getAttribute(hrefAttributeName);
        const absoluteHref = toAbsoluteUri(href);
        if (isWithinBaseUriSpace(absoluteHref)) {
            event.preventDefault();
            event.stopImmediatePropagation(); // we don't want the original blazor router to pick up this event.
            notifyNavigationAsync(SextantNavigationType.url, absoluteHref, null);
        }
    }
});
// The original blazor router will do the navigation.  It will pick up a navigation event after this event goes out.
function performInternalNavigation(absoluteInternalHref, interceptedLink) {
    console.log("Internal Navigation: " + absoluteInternalHref);
    notifyNavigationAsync(SextantNavigationType.forward, absoluteInternalHref, null);
}
function toAbsoluteUri(relativeUri) {
    testAnchor = testAnchor || document.createElement('a');
    testAnchor.href = relativeUri;
    return testAnchor.href;
}
function isWithinBaseUriSpace(href) {
    const baseUriWithTrailingSlash = toBaseUriWithTrailingSlash(document.baseURI); // TODO: Might baseURI really be null?
    return href.startsWith(baseUriWithTrailingSlash);
}
function toBaseUriWithTrailingSlash(baseUri) {
    return baseUri.substr(0, baseUri.lastIndexOf('/') + 1);
}
function findClosestAncestor(element, tagName) {
    return !element
        ? null
        : element.tagName === tagName
            ? element
            : findClosestAncestor(element.parentElement, tagName);
}
function eventHasSpecialKey(event) {
    return event.ctrlKey || event.shiftKey || event.altKey || event.metaKey;
}
// Add some details about the viewmodel so we can match it up to Sextant's stack.  Keep the same url so don't add the optional parameter.
// Ultimately, we can replace document.title with something more meaningful so browser history shows a more information.
function replaceState(state) {
    history.replaceState(state, document.title);
}
function goBack() {
    history.back(); // This will trigger popstate.
}
function goToRoot(count) {
    if (count >= 0)
        console.log("There's a problem.  GoToRoot should be called with a negative number in order to go back.");
    history.go(count); // This will trigger popstate.
}
//// this is a way to trick the browser into resending the popstate event so we can call a PopPage method from the viewstack when someone actually clicks back
//function undoBrowserBack() {
//    let currentState = history.state;
//    currentState.shouldHandleInternally = true;
//    history.pushState(null, document.title, location + "#");
//}
function notifyNavigationAsync(navigationType, url, state) {
    return __awaiter(this, void 0, void 0, function* () {
        if (notifyLocationChangedCallback) {
            yield window.DotNet.invokeMethodAsync(notifyLocationChangedCallback.assemblyName, notifyLocationChangedCallback.functionName, navigationType, url, state);
        }
    });
}
//function navigateTo(uri: string, forceLoad: boolean) {
//    const absoluteUri = toAbsoluteUri(uri);
//    if (!forceLoad && isWithinBaseUriSpace(absoluteUri)) {
//        // It's an internal URL, so do client-side navigation
//        performInternalNavigation(absoluteUri, false);
//    } else if (forceLoad && location.href === uri) {
//        // Force-loading the same URL you're already on requires special handling to avoid
//        // triggering browser-specific behavior issues.
//        // For details about what this fixes and why, see https://github.com/aspnet/AspNetCore/pull/10839
//        const temporaryUri = uri + '?';
//        history.replaceState(null, '', temporaryUri);
//        location.replace(uri);
//    } else {
//        // It's either an external URL, or forceLoad is requested, so do a full page load
//        location.href = uri;
//    }
//}
//# sourceMappingURL=navStack.js.map