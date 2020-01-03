let notifyLocationChangedCallback = { assemblyName: "Sextant.Blazor", functionName: "NotifyLocationState" };

let testAnchor;

interface Window {
    DotNet: DotNet;
}
interface DotNet {
    invokeMethodAsync<T>(methodIdentifier: string, ...args: any[]): Promise<T>;
}
Window

const SextantFunctions = {
    //registerForNavEvents,
    navigateTo,
    replaceState,
    getBaseUri: () => document.baseURI,
    getLocationHref: () => location.href,
    goBack,
    goToRoot
};

(<any>window).SextantFunctions = SextantFunctions;

// need this registered from the beginning!
window.addEventListener('popstate', (ev) => {
    console.log("Popstate event: " + ev.state);
    // ev.stopImmediatePropagation();
    if (ev.state == null || !ev.state.shouldHandleInternally) {
        notifyNavigation(true, location.href, ev.state);
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
            event.stopImmediatePropagation();
            //anchorTarget.getAttribute()
            // performInternalNavigation(absoluteHref, true);
            notifyNavigation(false, absoluteHref, null);
        }
    }
});

function performInternalNavigation(absoluteInternalHref, interceptedLink) {
    //history.pushState(null, /* ignored title */ '', absoluteInternalHref);
    console.log("Internal Navigation: " + absoluteInternalHref);
    notifyNavigation(false, absoluteInternalHref, null);
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
function replaceState(state) {
    history.replaceState(state, document.title);
}
function goBack() {
    history.back();
}

function goToRoot(count) {
    if (count >= 0)
        console.log("There's a problem.  GoToRoot should be called with a negative number in order to go back.");
    history.go(count);
}

// this is a way to trick the browser into resending the popstate event so we can call a PopPage method from the viewstack when someone actually clicks back
function undoBrowserBack() {
    let currentState = history.state;
    currentState.shouldHandleInternally = true;
    history.pushState(null, document.title, location + "#");
}

async function notifyNavigation(navigated, url, state) {
    if (notifyLocationChangedCallback) {
        await window.DotNet.invokeMethodAsync(notifyLocationChangedCallback.assemblyName, notifyLocationChangedCallback.functionName, navigated, url, state);
    }
}


function navigateTo(uri: string, forceLoad: boolean) {
    const absoluteUri = toAbsoluteUri(uri);

    if (!forceLoad && isWithinBaseUriSpace(absoluteUri)) {
        // It's an internal URL, so do client-side navigation
        performInternalNavigation(absoluteUri, false);
    } else if (forceLoad && location.href === uri) {
        // Force-loading the same URL you're already on requires special handling to avoid
        // triggering browser-specific behavior issues.
        // For details about what this fixes and why, see https://github.com/aspnet/AspNetCore/pull/10839
        const temporaryUri = uri + '?';
        history.replaceState(null, '', temporaryUri);
        location.replace(uri);
    } else {
        // It's either an external URL, or forceLoad is requested, so do a full page load
        location.href = uri;
    }
}
