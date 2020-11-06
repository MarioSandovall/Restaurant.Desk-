using Model.Dtos;
using Model.Models;
using Service.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace Service.Utils
{
    internal static class PurchaseDocument
    {
        private static BranchOffice _branchOffice;
        private static Restaurant _restaurant;
        private static ICollection<TicketDetailDto> _details;

        public static PrintDocument Generate(ICollection<TicketDetailDto> details, BranchOffice branchOffice, Restaurant restaurant)
        {
            var pd = new PrintDocument();
            var ps = new PaperSize { Width = MaxWidth };

            _details = details;
            _restaurant = restaurant;
            _branchOffice = branchOffice;

            pd.PrintPage -= PdOnPrintPage;
            pd.PrintPage += PdOnPrintPage;
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DefaultPageSettings.PaperSize = ps;

            return pd;
        }


        private static void PdOnPrintPage(object sender, PrintPageEventArgs e)
        {
            using (var g = e.Graphics)
            {
                var space = 2;
                var firstItem = _details.FirstOrDefault();

                var logo = _restaurant.Image.ArrayToImage();
                var image = logo.ResizeImage(70, 70).ToGrayScale();

                g.DrawRectangle(Pens.Black, 0, space, MaxWidth, 1);
                g.DrawImage(image, CenterImage(70), space += 5);
                g.DrawString(_restaurant.Name, TitleFont, Sb, new RectangleF(0, space += 75, MaxWidth, TitleSize + 5), CenterFormat);

                g.DrawString(_branchOffice.Name, TextFont, Sb, 0, space += 30);
                g.DrawString($"{_branchOffice.Street} {_branchOffice.OutdoorNumber}", TextFont, Sb, 0, space += 10);
                g.DrawString($"{_branchOffice.Suburb}, {_branchOffice.Town}", TextFont, Sb, 0, space += 10);

                g.DrawString($"Ticket: {firstItem?.OrderNumber}", TextFont, Sb, 0, space += 20);
                g.DrawString($"Fecha: {firstItem?.Date}", TextFont, Sb, 0, space += 10);
                g.DrawString($"Cajero: {firstItem?.Cashier}", TextFont, Sb, 0, space += 10);

                g.DrawRectangle(Pens.Black, 0, space += 20, MaxWidth, 1);

                AddHeader(g, space += 20, "Cantidad", "Descripcion", "Precio");

                foreach (var detail in _details)
                {
                    AddRow(g, space += 15, detail.Quantity, detail.Product, detail.Price);
                }

                g.DrawString($"Total  ${_details.Sum(x => x.Price * x.Quantity)}", TitleFont, Sb, new RectangleF(0, space += 30, MaxWidth, TitleSize + 5), FarFormat);
                g.DrawString("¡Gracias por su preferencia!", TextFont, Sb, new RectangleF(0, space += 30, MaxWidth, TextSize + 5), CenterFormat);
            }
        }

        private static void AddHeader(Graphics g, int space, string qty, string desc, string price)
        {
            const int size = MaxWidth / 3;
            g.DrawString($"{qty}", TextFont, Sb, new RectangleF(0, space, size, TextSize + 5), NearFormat);
            g.DrawString($"{desc}", TextFont, Sb, new RectangleF(size, space, size, TextSize + 5), CenterFormat);
            g.DrawString($"{price}", TextFont, Sb, new RectangleF(size * 2, space, size, TextSize + 5), FarFormat);
        }

        private static void AddRow(Graphics g, int space, int qty, string desc, decimal price)
        {
            const int size = MaxWidth / 3;
            g.DrawString($"{qty}", TextFont, Sb, new RectangleF(0, space, size, TextSize + 5), NearFormat);
            g.DrawString($"{desc}", TextFont, Sb, new RectangleF(size, space, size, TextSize + 5), NearFormat);
            g.DrawString($"${price}", TextFont, Sb, new RectangleF(size * 2, space, size, TextSize + 5), FarFormat);
        }

        private static int CenterImage(int width)
        {
            var value = (MaxWidth / 2) - (width / 2);
            return value < 0 ? 0 : value;
        }

        private const int TextSize = 7;
        private const int TitleSize = 8;
        private const int MaxWidth = 184;
        private const string FontName = "Segoe UI";

        private static readonly Font TitleFont = new Font(FontName, TitleSize, FontStyle.Bold);
        private static readonly Font TextFont = new Font(FontName, TextSize, FontStyle.Regular);
        private static readonly SolidBrush Sb = new SolidBrush(Color.Black);

        private static readonly StringFormat CenterFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
        private static readonly StringFormat NearFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
        private static readonly StringFormat FarFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
    }
}
