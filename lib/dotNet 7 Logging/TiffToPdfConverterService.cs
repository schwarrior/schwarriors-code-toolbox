using Microsoft.Extensions.Options;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace ISEEImageToPdfConverter;

public interface ITiffToPdfConverterService
{
    public IOptions<AppSettings> AppSettings { get; set; }

    public int ConvertImages(Log log);

    public int ConvertSingleImage(string inFileName);
}

public class TiffToPdfConverterService: ITiffToPdfConverterService
{
    public IOptions<AppSettings> AppSettings { get; set; }

    public TiffToPdfConverterService(IOptions<AppSettings> appSettings) 
    {
        AppSettings = appSettings;
    }

    const string LogRunDelimitter = "===============================";

    public int ConvertImages(Log log)
    {
        var returnCode = 0;
        try
        {
            log.LogInfo(LogRunDelimitter);
            Thread.Sleep(100);
            log.LogInfo("Image to PDF Converter Starting");
            log.TraceEnvironmentInfo();
            var appSettings = AppSettings.Value;
            Database db = new Database(appSettings.ConnectionString ?? "empty", appSettings.Query ?? "empty");
            List<ImageRecord> imageRecords = db.getImageRecords();
            List<int> rowNumOfSuccessufullyConvertedImage = new List<int>();

            if (imageRecords.Count > 0)
            {
                log.LogNumberOfImagesToBeConverted(imageRecords.Count);

                foreach (ImageRecord record in imageRecords)
                {
                    switch (ConvertSingleImage(record.imageName))
                    {
                        case 0:
                            log.LogSuccessfulImageConversion(record.imageName);
                            rowNumOfSuccessufullyConvertedImage.Add(record.id);
                            break;
                        case 1:
                            log.LogFailedConversionException(record.imageName);
                            break;
                        case 2:
                            log.LogFailedConversionFileDoesNotExist(record.imageName);
                            break;
                        default:
                            log.LogUnknownFailure(record.imageName);
                            break;
                    }

                }
            }
            else
            {
                log.LogNoImagesPending();
            }

            if (rowNumOfSuccessufullyConvertedImage.Count > 0)
            {
                log.LogDeletingSuccessfullyConvertedRows();
                db.DeleteSuccessfullyConvertedRows(rowNumOfSuccessufullyConvertedImage);
            }

            log.LogInfo("Image to PDF Converter Done");
        }
        catch (Exception ex)
        {
            log?.LogCritical(ex);
            returnCode = ex.HResult == 0 ? -1 : ex.HResult;
            log?.LogInfo("Image to PDF Converter Completed with a Failure");
        }

        Thread.Sleep(100);
        log?.LogInfo(LogRunDelimitter);

        return returnCode;
    }

    public int ConvertSingleImage(string inFileName)
    {
        string outFileName = CreateOutFileName(inFileName);
        try
        {
            if (File.Exists(inFileName))
            {
                PdfWriter writer = new PdfWriter(outFileName);
                PdfDocument pdfDocument = new PdfDocument(writer);
                Document document = new Document(pdfDocument, new iText.Kernel.Geom.PageSize(612.0f, 792.0f));
                ImageData data = ImageDataFactory.Create(inFileName);
                Image img = new Image(data);
                document.Add(img);
                document.Close();
            }
            else
            {
                return 2;
            }

        }
        catch
        {
            return 1;
        }



        return 0;
    }

    private string CreateOutFileName(string inFileName)
    {
        return $"{inFileName}.pdf";
    }

}
