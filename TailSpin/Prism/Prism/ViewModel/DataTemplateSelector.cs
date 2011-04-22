namespace Microsoft.Practices.Prism.ViewModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    ///   This custom ContentControl changes its ContentTemplate based on the content it is presenting.
    /// </summary>
    /// <remarks>
    ///   In order to determine the template it must use for the new content, this control retrieves it from its
    ///   resources using the name for the type of the new content as the key.
    /// </remarks>
    public class DataTemplateSelector : ContentControl
    {
        /// <summary>
        ///   Returns the default content template to use if not other content template can be located.
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        protected virtual DataTemplate GetDefaultContentTemplate()
        {
            return null;
        }

        /// <summary>
        ///   Called when the value of the <see cref = "P:System.Windows.Controls.ContentControl.Content" /> property changes.
        /// </summary>
        /// <param name = "oldContent">The old value of the <see cref = "P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        /// <param name = "newContent">The new value of the <see cref = "P:System.Windows.Controls.ContentControl.Content" /> property.</param>
        /// <remarks>
        ///   Will attempt to discover the <see cref = "DataTemplate" /> from the <see cref = "ResourceDictionary" /> by matching the type name of <paramref name = "newContent" />.
        /// </remarks>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            DataTemplate contentTemplate = this.GetDefaultContentTemplate();
            if (newContent != null)
            {
                var contentTypeName = newContent.GetType().Name;
                contentTemplate = this.Resources[contentTypeName] as DataTemplate;
            }

            this.ContentTemplate = contentTemplate;
        }
    }
}