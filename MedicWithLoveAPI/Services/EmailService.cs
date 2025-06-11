using MedicWithLoveAPI.Models;

using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Xls;

using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace MedicWithLoveAPI.Services;

public class EmailService : IDisposable
{
	private readonly MailAddress from = new("saihonfox@yandex.ru", "Medic", Encoding.UTF8);
	private readonly SmtpClient client = new("smtp.yandex.ru", 25)
	{
		Credentials = new NetworkCredential(
			"saihonfox@yandex.ru",
			Environment.GetEnvironmentVariable("yandex_smtp_password", EnvironmentVariableTarget.Machine)!
		),
		EnableSsl = true,
	};

	public async void SendEmail(string email)
	{
		using var message = new MailMessage(from, new(email)) { IsBodyHtml = false, BodyEncoding = Encoding.UTF8, Subject = "che da" };
		message.Body = "hi";

		await client.SendMailAsync(message);
	}

	public async void SendEmailPatientOrder(string email, AnalysisOrder order)
	{
		using var message = new MailMessage(from, new(email)) { IsBodyHtml = false, BodyEncoding = Encoding.UTF8, Subject = "Запись на анализы" };

		var sb = new StringBuilder();
		sb.AppendLine($"Вы успешно записаны на сдачу анализов на {order.AnalysisDatetime:dd.MM.yyyy HH:mm}");
		sb.AppendLine($"По адресу: {(order.AtHome ? order.Patient!.Address : "Клиника")}");
		sb.AppendLine();
		if (!string.IsNullOrWhiteSpace(order.Comment))
			sb.AppendLine($"Ваш комментарий: {order.Comment}");
		sb.AppendLine("< Анализы ");
		sb.AppendLine($"| на общую сумму {order.PatientAnalysisCart!.PatientAnalysisCartItems.ToList().Sum(x => x.Analysis.Price).ToString("0.00")} руб.");
		sb.AppendLine("#");

		int maxNameLen = order.PatientAnalysisCart!.PatientAnalysisCartItems.Max(x => x.Analysis.Name.Length);
		int maxPriceLen = order.PatientAnalysisCart!.PatientAnalysisCartItems.Max(x => x.Analysis.Price.ToString("0.00").Length);
		foreach (var cartItem in order.PatientAnalysisCart!.PatientAnalysisCartItems)
		{
			sb.AppendLine($"|> {cartItem.Analysis.Name.PadLeft(Math.Abs(maxNameLen - cartItem.Analysis.Name.Length))} - {cartItem.Analysis.Price.ToString("0.00").PadRight(Math.Abs(cartItem.Analysis.Price.ToString("0.00").Length - maxPriceLen))} руб.\nРезультат: {cartItem.ResultsDescription}");
		}
		sb.AppendLine("< " + new string('#', 10));
		sb.AppendLine();

		sb.AppendLine($"Врач: {order.User!.FullName}");

		message.Body = sb.ToString();

		string fileName = Environment.CurrentDirectory + $"/MedicInPoint/temp_excel.xlsx";
		CreateExcelFile(order, fileName);
		using var mw = new MemoryStream(await File.ReadAllBytesAsync(fileName), true);
		message.Attachments.Add(new Attachment(mw, "cheque.xlsx", "application/vnd.ms-excel"));
		File.Delete(fileName);

		try
		{
			await client.SendMailAsync(message);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Message: {ex.Message}");
		}
		finally
		{
			await mw.FlushAsync();
		}
	}

	void CreateExcelFile(AnalysisOrder order, string fileName)
	{
		var workBook = new Workbook();
		var sheet = workBook.Worksheets[0];

		int maxNameLen = order.PatientAnalysisCart!.PatientAnalysisCartItems.Max(x => x.Analysis.Name.Length);
		int maxPriceLen = order.PatientAnalysisCart!.PatientAnalysisCartItems.Max(x => x.Analysis.Price.ToString("0.00").Length);

		List<string> list = [
			$"Запись на сдачу анализов на {order.AnalysisDatetime:dd.MM.yyyy HH:mm}",
			"< Анализы ",
			$"| на общую сумму {order.PatientAnalysisCart!.PatientAnalysisCartItems.ToList().Sum(x => x.Analysis.Price):0.00} руб.",
			"#",
		];
		if (!string.IsNullOrWhiteSpace(order.Comment))
			list.InsertRange(1, "", $"Комментарий пациента: {order.Comment}", "");
		list.AddRange(order.PatientAnalysisCart!.PatientAnalysisCartItems.Select(cartItem => $"|> {cartItem.Analysis.Name.PadLeft(Math.Abs(maxNameLen - cartItem.Analysis.Name.Length))} - {cartItem.Analysis.Price.ToString("0.00").PadRight(Math.Abs(cartItem.Analysis.Price.ToString("0.00").Length - maxPriceLen))} руб.\nРезультат: {cartItem.ResultsDescription}"));
		list.Add("< " + new string('#', 10));
		list.Add("");
		list.Add($"Лаборант: {order.User.FullName}");
		list.Add($"Пациент: {order.Patient.FullName}");
		list.Add($"Место проведения: {(order.AtHome ? "Дома у клиента" : "Клиника")}");
		for (int i = 1; i <= list.Count; i++)
			sheet.Range[$"A{i}"].Value = list[i - 1];
		sheet.Columns[0].AutoFitColumns();
		sheet.Columns[0].AutoFitRows();

		sheet.SaveToFile(fileName, ",");
	}

	public async void SendEmailWithAttachments(string email, string filePath, string mediaType)
	{
		using var message = new MailMessage(from, new(email)) { IsBodyHtml = false, BodyEncoding = Encoding.UTF8, Subject = "" };
		message.Body = "hi";

		using var document = new PdfDocument();
		var page = document.Pages.Add();
		page.Canvas.DrawString("zig", new PdfFont(PdfFontFamily.Helvetica, 16), new PdfSolidBrush(Color.Aqua), 0, 0);
		document.SaveToFile("temp_order.pdf", Spire.Pdf.FileFormat.PDF);
		document.Close();

		using var mw = new MemoryStream(await File.ReadAllBytesAsync("temp_order.pdf"), true);
		File.Delete("temp_order.pdf");

		using var attachment = new Attachment(mw, "order.pdf", MediaTypeNames.Application.Pdf);
		message.Attachments.Add(attachment);

		await mw.FlushAsync();

		await client.SendMailAsync(message);
	}

	// Отчеты врачей
	void Admin_DoctorsReports()
	{

	}

	// Отчеты пациентов
	void Admin_PatientsReports()
	{

	}

	// Отчеты результатов анализов
	void Admin_AnalysesReports()
	{

	}

	// Отчеты результатов анализов определенного врача
	void Doctor_Reports(User doctor)
	{

	}

	public void Dispose()
	{
		client.Dispose();
		GC.SuppressFinalize(this);
	}
}