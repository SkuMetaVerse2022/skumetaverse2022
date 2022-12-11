using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView.Internal;
using Vuplex.WebView;
public class JavascriptExcute : MonoBehaviour
{    
    async void Start()
    {
        var webViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        // Wait for the WebViewPrefab to initialize, because the WebViewPrefab.WebView property
        // is null until the prefab has initialized.
            //scriptElement.innerText = 'document.querySelectorAll(`[data-a-target=\'player-fullscreen-button\']`)[0].click();';
        await webViewPrefab.WaitUntilInitialized();
        webViewPrefab.WebView.PageLoadScripts.Add(@"document.getElementsByClassName('top-nav')[0].remove();document.getElementsByClassName('side-nav__scrollable_content')[0].remove();document.getElementById('sideNav').remove();document.getElementsByClassName('channel-root__right-column')[0].remove();document.getElementsByClassName('channel-info-content')[0].remove();document.getElementById('twilight-sticky-footer-root').remove();document.getElementsByClassName('home')[0].remove();document.body.style.backgroundColor='black';document.getElementsByClassName('channel-root__info--offline')[0].style.display='none';document.getElementsByClassName('persistent-player')[0].style.display='none';document.querySelectorAll('video, audio').forEach(mediaElement => mediaElement.volume = 0.75);window.addEventListener('keydown', (m) => {console.log(m);});window.dispatchEvent(new KeyboardEvent('keydown', { key: 'm', keyCode: 77, code: 'KeyM',  which: 77, shiftKey: false, ctrlKey: false, metaKey: false}));
        
        ");
    }
}
