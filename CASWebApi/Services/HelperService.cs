
using CASWebApi.IServices;
using CASWebApi.Models;
using GemBox.Spreadsheet;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;

/// <summary>
/// Helper Service will contain additional functions like Uploading image, Export/ Import files and etc.
/// </summary>
namespace CASWebApi.Services
{
    public class HelperService
    {
        IDbSettings DbContext;

        public HelperService(IDbSettings settings)
        {
            DbContext = settings;
        }

        /// <summary>
        /// Function to fetch all student data
        /// </summary>
        /// <param name="className">Collection name to fecth all data</param>
        /// <returns></returns>
        public List<Student> GetAll(string collectionName)
        {
            return DbContext.GetAll<Student>(collectionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public byte[] ToExcelFile(List<Student> collection)
        {
            //Creating Excel file to export the data
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            GemBox.Spreadsheet.ExcelWorksheet worksheet = workbook.Worksheets.Add("Writing");
            worksheet.Cells["A1"].Value = "Id";
            worksheet.Cells["B1"].Value = "First Name";
            worksheet.Cells["C1"].Value = "Last Name";
            worksheet.Cells["D1"].Value = "Email";
            worksheet.Cells["E1"].Value = "Phone";
            worksheet.Cells["F1"].Value = "Gender";
            worksheet.Cells["G1"].Value = "Birthday";
            worksheet.Cells["H1"].Value = "City";
            worksheet.Cells["I1"].Value = "Street";
            worksheet.Cells["J1"].Value = "ZipCode";
            worksheet.Cells["K1"].Value = "Group";

            int row = 2;
            //Creating Excel coloumns
            foreach (var item in collection)
            {
                worksheet.Cells[string.Format("A{0}", row)].Value = item.Id;
                worksheet.Cells[string.Format("B{0}", row)].Value = item.First_name;
                worksheet.Cells[string.Format("C{0}", row)].Value = item.Last_name;
                worksheet.Cells[string.Format("D{0}", row)].Value = item.Email;
                worksheet.Cells[string.Format("E{0}", row)].Value = item.Phone;
                worksheet.Cells[string.Format("F{0}", row)].Value = item.Gender;
                worksheet.Cells[string.Format("G{0}", row)].Value = item.Birth_date;
                worksheet.Cells[string.Format("H{0}", row)].Value = item.Address.City;
                worksheet.Cells[string.Format("I{0}", row)].Value = item.Address.Street;
                worksheet.Cells[string.Format("J{0}", row)].Value = item.Address.ZipCode;
                worksheet.Cells[string.Format("K{0}", row)].Value = item.Group_Id;
                row++;
            }

            var stream = new MemoryStream();
            var excelOptions = SaveOptions.XlsDefault;
            workbook.Save(stream, excelOptions);
            using (var excelStream = stream)
            return excelStream.ToArray();

        }


    }
}
