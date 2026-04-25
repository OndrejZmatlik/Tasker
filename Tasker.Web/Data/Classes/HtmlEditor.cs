using MudExRichTextEditor.Types;

namespace Tasker.Web.Data.Classes
{
    public class HtmlEditor
    {
        public static IEnumerable<QuillTool> All()
        {
            yield return new QuillTool("ql-header", "", 1, new string[7] { "", "1", "2", "3", "4", "5", "6" });
            yield return new QuillTool("ql-bold", "", 2);
            yield return new QuillTool("ql-italic", "", 2);
            yield return new QuillTool("ql-underline", "", 2);
            yield return new QuillTool("ql-strike", "", 2);
            yield return new QuillTool("ql-color", "", 3, Array.Empty<string>());
            yield return new QuillTool("ql-background", "", 3, Array.Empty<string>());
            yield return new QuillTool("ql-list", "ordered", 4);
            yield return new QuillTool("ql-list", "bullet", 4);
            yield return new QuillTool("ql-indent", "-1", 4);
            yield return new QuillTool("ql-indent", "+1", 4);
            yield return new QuillTool("ql-align", "", 4, new string[4] { "", "center", "right", "justify" });
            yield return new QuillTool("ql-blockquote", "", 5);
            //yield return new QuillTool("ql-code-block", "", 5);
            yield return new QuillTool("ql-link", "", 6);
            //yield return new QuillTool("ql-image", "", 6);
            //yield return new QuillTool("ql-video", "", 6);
        }
    }
}
