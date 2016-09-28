using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Services.Manuscript.ViewModel;
using HtmlAgilityPack;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Utilities.HtmlUtilityPack;

namespace TransferDesk.Services.Manuscript.Preview
{
   public class ManuscriptBookScreeningPreview
    {
        private ManuscriptBookScreeningVm _manuscriptBookScreeningVm;
        private HTMLToText _htmlToText;
        public List<BookMaster> BookTitleList { get; set; }


        public ManuscriptBookScreeningPreview(ManuscriptBookScreeningVm manuscriptBookScreeningVmScreeningVM)
        {
              _htmlToText=new HTMLToText();
              _manuscriptBookScreeningVm = manuscriptBookScreeningVmScreeningVM;
        }

        public string GetBookTitleList(int? ID)
        {
            if (ID == null) return string.Empty;

            foreach (BookMaster booktitle in BookTitleList)
            {
                if (booktitle.ID == _manuscriptBookScreeningVm.BookTitleId)
                {
                    return booktitle.BookTitle;
                }
            }
            return string.Empty;
        }
        public string CreateHtmlPreview(List<HtmlRowData> htmlNodeDataList, string parentNodename, bool asHtmlPage, string tableClassName)
        {
            var doc = new HtmlDocument();
           const string body = "Body";
            if (asHtmlPage == true)
            {
                parentNodename = body;
            }

            var parentNode = doc.CreateElement(parentNodename);

            var tableNode = doc.CreateElement("Table class='" + tableClassName + "'");

            parentNode.AppendChild(tableNode);

            foreach (HtmlRowData htmlNodeData in htmlNodeDataList)
            {
                CreateAppendRow(doc, tableNode, htmlNodeData);
            }


            if (asHtmlPage == true)
            {
                string testHtml = "<HTML><Head><meta charset='UTF-8'><Head><Body>" + tableNode.OuterHtml + "</Body></HTML>";
                return testHtml;
            }
            else
            {
                return doc.DocumentNode.OuterHtml;
            }
        }

        private void CreateAppendRow(HtmlDocument doc, HtmlNode appendToTableNode, HtmlRowData htmlRowData)
        {
            if (htmlRowData.InnerHtml == null)
            {
                htmlRowData.InnerHtml = string.Empty;
            }
            var rowNode = doc.CreateElement("TR class ='" + htmlRowData.StyleClass + "'");
            var rowDivLabelNode = doc.CreateElement("TD");
            var rowLabelNode = doc.CreateElement("Div");
            rowLabelNode.InnerHtml = htmlRowData.LabelText;
            rowDivLabelNode.AppendChild(rowLabelNode);
            rowNode.AppendChild(rowDivLabelNode);
            var rowDivTextNode = doc.CreateElement("TD");
            var textColumnNode = doc.CreateElement("Div");
            textColumnNode.InnerHtml = htmlRowData.InnerHtml;
            rowDivTextNode.AppendChild(textColumnNode);
            rowNode.AppendChild(rowDivTextNode);
            appendToTableNode.AppendChild(rowNode);
        }

    }
  
}
