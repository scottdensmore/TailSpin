namespace Microsoft.Practices.Prism.Interactivity
{
    using System;
    using System.Linq;
    using Microsoft.Phone.Shell;

    /// <summary>
    ///   Extensions to the <see cref = "IApplicationBar" />.
    /// </summary>
    public static class ApplicationBarExtensions
    {
        ///<summary>
        ///  Finds an <see cref = "ApplicationBarIconButton" /> by its name.
        ///</summary>
        ///<param name = "appBar"></param>
        ///<param name = "text"></param>
        ///<returns></returns>
        [CLSCompliant(false)]
        public static ApplicationBarIconButton FindButton(this IApplicationBar appBar, string text)
        {
            if (appBar == null)
            {
                throw new ArgumentNullException("appBar");
            }
            return (from object button in appBar.Buttons select button as ApplicationBarIconButton).FirstOrDefault(btn => btn != null && btn.Text == text);
        }
    }
}