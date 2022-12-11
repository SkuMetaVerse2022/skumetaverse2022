using UnityEngine;
using UnityEngine.UI;

namespace Paroxe.PdfRenderer.Examples
{
    public class PDFDocumentRenderToTextureExample : MonoBehaviour
    {
        public int m_Page = 0;

        private int width = 1700;
        private int height = 950;

        public bool scaleBtn;

        public InputField pathInputField;
        public InputField FileInputField;

#if !UNITY_WEBGL
        void Start()
        {

            
        }

        public void Onclicked()
        {
            PDFDocument pdfDocument = new PDFDocument(pathInputField.text + "/" + FileInputField.text);

            if (pdfDocument.IsValid)
            {
                int pageCount = pdfDocument.GetPageCount();

                PDFRenderer renderer = new PDFRenderer();
                Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(m_Page % pageCount), width, height);

                tex.filterMode = FilterMode.Point;
                tex.anisoLevel = 8;

                GetComponent<MeshRenderer>().material.mainTexture = tex;
            }
        }

        public void OnNextPageClicked()
        {
            PDFDocument pdfDocument = new PDFDocument(pathInputField.text + "/" + FileInputField.text);

            if (pdfDocument.IsValid)
            {
                int pageCount = pdfDocument.GetPageCount();

                m_Page += 1;
                PDFRenderer renderer = new PDFRenderer();
                Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(m_Page % pageCount), width, height);

                tex.filterMode = FilterMode.Bilinear;
                tex.anisoLevel = 8;

                GetComponent<MeshRenderer>().material.mainTexture = tex;
            }
        }

        public void OnPrevPageClicked()
        {
            if(m_Page == 0)
            {
                m_Page = 0;
            }
            else
            {
                m_Page -= 1;
            }

            PDFDocument pdfDocument = new PDFDocument(pathInputField.text + "/" + FileInputField.text);

            if (pdfDocument.IsValid)
            {
                int pageCount = pdfDocument.GetPageCount();

                PDFRenderer renderer = new PDFRenderer();
                Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(m_Page % pageCount), width, height);

                tex.filterMode = FilterMode.Bilinear;
                tex.anisoLevel = 8;

                GetComponent<MeshRenderer>().material.mainTexture = tex;
            }
        }

        public void OnPageScaleClicked()
        {
            scaleBtn = true;
            width = 1700;
            height = 950;
            transform.localScale = new Vector3(14.3f, 1f, 8f);

            PDFDocument pdfDocument = new PDFDocument(pathInputField.text + "/" + FileInputField.text);

            if (pdfDocument.IsValid)
            {
                int pageCount = pdfDocument.GetPageCount();

                PDFRenderer renderer = new PDFRenderer();
                Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(m_Page % pageCount), width, height);

                tex.filterMode = FilterMode.Point;
                tex.anisoLevel = 8;

                GetComponent<MeshRenderer>().material.mainTexture = tex;
            }
        }

        public void OffPageScaleClicked()
        {
            scaleBtn = false;
            width = 1200;
            height = 1700;
            transform.localScale = new Vector3(10f, 1f, 15f);

            PDFDocument pdfDocument = new PDFDocument(pathInputField.text + "/" + FileInputField.text);

            if (pdfDocument.IsValid)
            {
                int pageCount = pdfDocument.GetPageCount();

                PDFRenderer renderer = new PDFRenderer();
                Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(m_Page % pageCount), width, height);

                tex.filterMode = FilterMode.Point;
                tex.anisoLevel = 8;

                GetComponent<MeshRenderer>().material.mainTexture = tex;
            }
        }
#endif
    }
}
