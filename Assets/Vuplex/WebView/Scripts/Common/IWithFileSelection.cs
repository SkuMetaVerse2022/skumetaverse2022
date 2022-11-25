/**
* Copyright (c) 2021 Vuplex Inc. All rights reserved.
*
* Licensed under the Vuplex Commercial Software Library License, you may
* not use this file except in compliance with the License. You may obtain
* a copy of the License at
*
*     https://vuplex.com/commercial-library-license
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
using System;

namespace Vuplex.WebView {

    /// <summary>
    /// An interface implemented by a webview if it supports file selection.
    /// </summary>
    /// <example>
    /// <code>
    /// await webViewPrefab.WaitUntilInitialized();
    /// var webViewWithFileSelection = webViewPrefab.WebView as IWithFileSelection;
    /// if (webViewWithFileSelection != null) {
    ///     webViewWithFileSelection.FileSelectionRequested += (sender, eventArgs) => {
    ///         // Note: You should show a file picker to the user so they can pick the file rather
    ///         //       than using a hardcoded file path like demonstrated here.
    ///         var filePaths = new string[] { "C:\\Users\\YourUser\\Desktop\\selected-file.txt" };
    ///         eventArgs.Continue(filePaths);
    ///     };
    /// }
    /// </code>
    /// </example>
    public interface IWithFileSelection {

        /// <summary>
        /// Indicates that the page requested a file selection dialog. This can happen, for example, when a file input
        /// is activated. Call the event args' Continue(filePaths) to provide a file selection or call
        /// Cancel() to cancel file selection.
        /// </summary>
        event EventHandler<FileSelectionEventArgs> FileSelectionRequested;
    }
}
