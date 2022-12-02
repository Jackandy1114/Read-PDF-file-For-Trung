using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

class Program
{
    public static List<GiaoDich> cacGiaoDich = new();
    public static void Main()
    {


        Console.InputEncoding = System.Text.Encoding.Unicode;//Để nhập tiếng việt
        Console.OutputEncoding = System.Text.Encoding.Unicode;//Để xuất tiếng việt
        for (int i = 0; i < 49; i++)
        {
            cacGiaoDich.Add(XuLyPDF(@$"D:\pdf\result{i}.pdf"));
        }




    }
    static GiaoDich XuLyPDF(string filePath = @"D:\pdf\result2.pdf")
    {
        var src = @$"{filePath}";
        var pdfDocument = new PdfDocument(new PdfReader(src));
        string text_InPDF = "";
        var lastpage = pdfDocument.GetNumberOfPages();
        for (int i = 1; i <= lastpage; ++i)
        {
            var page = pdfDocument.GetPage(i);
            var strategy = new LocationTextExtractionStrategy();

            string text = PdfTextExtractor.GetTextFromPage(page, strategy);
            text = text.Replace("Evaluation Warning : The document was created with Spire.PDF for .NET.", "");
            text = text.Replace("Bản quyền thuộc Sở Giao dịch Chứng khoán Hà Nội.", "\n");
            text = text.Replace("1.Tổng KLGD Tự doanh = Tổng KLGD mua Tự doanh + Tổng KLGD bán Tự doanh", "");
            text = text.Replace("2.Tổng GTGD Tự doanh = Tổng GTGD mua Tự doanh + Tổng GTGD bán Tự doanh", "");
            text = text.Replace("Đơn vị: đồng", "");
            text = text.Replace("STT Mã CK Tổng KLGD mua Tự doanh Tổng GTGD mua Tự doanh Tổng KLGD bán Tự doanh Tổng GTGD bán Tự doanh Tổng KLGD Tự doanh Tổng GTGD Tự doanh\n", "");
            text = text.Replace("*Ghi chú:", "");
            if (i == 1)
            {
                var s = text.Split("________________________________________________________________________________________________________________________________________________________\n");
                text = s[1];
            }
            if (i == lastpage)
            {
                text = text.Remove(text.Length - 3, 3);
            }
            text_InPDF += text;
        }
        pdfDocument.Close();
        var contentGiaoDich = text_InPDF.Split('\n').ToList();
        var name = contentGiaoDich[0];
        contentGiaoDich.RemoveRange(0, 4);

        var dsNoiDung = new List<noiDung>();
        for (int i = 0; i < contentGiaoDich.Count - 2; i++)
        {
            dsNoiDung.Add(XuLyNoiDung(contentGiaoDich[i]));
        }

        return new GiaoDich()
        {
            name = name,
            content = dsNoiDung,
        };
    }
    public static noiDung XuLyNoiDung(string content = @"1 BPC 0 0 60 468.000 60 468.000")
    {
        var temp = content.Split(' ');
        if (temp[0] == "Tổng")
        {
            return new noiDung()
            {
                STT = "Phú Đạt😊",
                MaCK = temp[0],
                TongKLGDMua = temp[1],
                TongGTGDMua = temp[2],
                TongKLGDBan = temp[3],
                TongGTGDBan = temp[4],
                TongKLGDTuDoanh = temp[5],
                TongGTGDTuDoanh = temp[6],
            };
        }
        else
        {

            return new noiDung()
            {
                STT = temp[0],
                MaCK = temp[1],
                TongKLGDMua = temp[2],
                TongGTGDMua = temp[3],
                TongKLGDBan = temp[4],
                TongGTGDBan = temp[5],
                TongKLGDTuDoanh = temp[6],
                TongGTGDTuDoanh = temp[7],
            };
        }
    }
}


class GiaoDich
{
    public string name { get; set; }
    public List<noiDung> content { get; set; }
}

class noiDung
{
    public string STT { get; set; }
    public string MaCK { get; set; }
    public string TongKLGDMua { get; set; }
    public string TongGTGDMua { get; set; }
    public string TongKLGDBan { get; set; }
    public string TongGTGDBan { get; set; }
    public string TongKLGDTuDoanh { get; set; }
    public string TongGTGDTuDoanh { get; set; }

}

//STT Mã CK Tổng KLGD mua Tự doanh Tổng GTGD mua Tự doanh Tổng KLGD bán Tự doanh Tổng GTGD bán Tự doanh Tổng KLGD Tự doanh Tổng GTGD Tự doanh