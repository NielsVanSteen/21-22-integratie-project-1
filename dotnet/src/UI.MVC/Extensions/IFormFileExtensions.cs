using System.Text;
using System.Text.RegularExpressions;
using Aspose.Pdf;
using SautinSoft;

namespace UI.MVC.Extensions;

/// <author> Michiel Verschueren </author>
/// <summary>
/// Extensions class that holds extensions for the <see cref="IFormFile"/> type
/// </summary>
public static class FormFileExtensions
{
    /// <summary>
    /// Method that parses a <see cref="IFormFile"/> which holds a pdf file to a html string
    /// </summary>
    /// <param name="file">The pdf to be parsed</param>
    /// <param name="hostingEnvironment">The current <see cref="IWebHostEnvironment"/> of the request</param>
    /// <param name="ct"><see cref="CancellationToken"/> that is passed down from the invoker to tell if the current request is cancelled</param>
    /// <returns>A html string</returns>
    /// <exception cref="FileLoadException">When something went wrong with the file loading</exception>
    public static async Task<string> ParseToPdf(this IFormFile file, IWebHostEnvironment hostingEnvironment, CancellationToken ct)
    {
        //Make a uploads directory
        string uploads = Path.Combine(hostingEnvironment.ContentRootPath, "Uploads");;
        Directory.CreateDirectory(uploads);
        
        //make PdfFocus object to be used to parse the pdf
        PdfFocus f = new PdfFocus();

        //If the length of the file is less than or equal to 0 throw an exception
        if (file.Length <= 0)
        {
            throw new FileLoadException("File was loaded incorrectly");
        }
        
        //Load the file using a memorystream
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            f.OpenPdf(ms.ToArray());
        }
        
        //Set different options
        f.HtmlOptions.InlineCSS = true;
        f.HtmlOptions.IncludeImageInHtml = true;
        f.HtmlOptions.ProduceOnlyHtmlBody = true;
        
        //Output the pdf as html
        f.ToHtml(Path.Combine(uploads, "Upload.html"));
        
        byte[] result;
        // Open the file again to read it as a string
        var filePath = Path.Combine(uploads, "Upload.html");
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            result = new byte[fileStream.Length];
            var bytesRead = await fileStream.ReadAsync(result, 0, (int)fileStream.Length, ct);
        }
        // Read the file
        var resultString = Encoding.ASCII.GetString(result);
        
        // prepare Regex pattern
        var pattern =
            "(\\?\\?\\?)(.*)(<div style=\\\"position:absolute; left:\\d{1,}\\.\\d{1,}pt; top:\\d{1,}\\.\\d{1,}pt;\\\"><span style=\\\"position:absolute; white-space:pre; font:\\d{1,}pt 'Calibri'; color:#\\d{1,}; font-weight:bold; left:\\d{1,}pt\\\">PDF Focus.*)";
        var aTags = "<a name=\"Page[0-9]{1,}\" id=\"Page[0-9]{1,}\"\\/>";
        //Remove LineEndings
        resultString = resultString.ReplaceLineEndings("");
        Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
        
        //Remove the matches from the imported string
        var match = r.Match(resultString);
        while (match.Success)
        {
            var resultString1 = resultString.Remove(match.Groups[1].Index, match.Groups[1].Length);
            var resultString2 = resultString1.Remove(match.Groups[3].Index);
            resultString = resultString2;
            match = match.NextMatch();
        }

        Regex regex2 = new Regex(aTags, RegexOptions.IgnoreCase);
        resultString = regex2.Replace(resultString, string.Empty);
        
        //Close the pdf
        f.ClosePdf();

        //Recursively delete the uploads directory
        // Directory.Delete(uploads, true);

        //Return the html string
        return resultString;
    }

    public static async Task<string> ParseToPdftemp(this IFormFile file, IWebHostEnvironment hostingEnvironment,
        CancellationToken ct)
    {
        //Make a uploads directory
        string uploads = Path.Combine(hostingEnvironment.ContentRootPath, "Uploads");
        
        Directory.CreateDirectory(uploads);
        
        //If the length of the file is less than or equal to 0 throw an exception
        if (file.Length <= 0)
        {
            throw new FileLoadException("File was loaded incorrectly");
        }
        
        
        // Load source PDF file
        Document doc;
        //Load the file using a memorystream
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms, ct);
            doc = new Document(ms);
            
            // Instantiate HTML Save options object
            HtmlSaveOptions newOptions = new HtmlSaveOptions();
        
            // Enable option to embed all resources inside the HTML
            newOptions.PartsEmbeddingMode = HtmlSaveOptions.PartsEmbeddingModes.EmbedAllIntoHtml;
            newOptions.RasterImagesSavingMode = HtmlSaveOptions.RasterImagesSavingModes.AsEmbeddedPartsOfPngPageBackground;
        
            // save document in HTML format with the given options
            doc.Save(Path.Combine(uploads, "Upload.html"), newOptions);
            
        }


        byte[] result;
        // Open the file again to read it as a string
        var filePath = Path.Combine(uploads, "Upload.html");
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            result = new byte[fileStream.Length];
            var bytesRead = await fileStream.ReadAsync(result, 0, (int)fileStream.Length, ct);
        }
        // Read the file
        var resultString = Encoding.ASCII.GetString(result);

        // var filter =
        //     "<div class=\"stl_01\" style=\"left:1.6667em;top:0.6559em;\"><span class=\"stl_07 stl_08 stl_09\">Evaluation Only. Created with Aspose.PDF. Copyright 2002-2022 Aspose Pty Ltd. &nbsp;</span></div>";
        //     resultString = resultString.Replace(filter, string.Empty);
        
        return resultString.ReplaceLineEndings(string.Empty);
    }
    
}