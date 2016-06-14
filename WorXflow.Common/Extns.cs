using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using AEonAX.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WorXflow.Common
{
    public static class RtbExtns
    {
        private static TableRowGroup TX;
        public static void PrintMessage(this RichTextBox rtbMessages, string Name, string Message, Data.Type MsgType)
        {
            rtbMessages.WPFUIize(() =>
            {
                if (TX == null)
                {
                    var msgTable = new Table();
                    rtbMessages.Document.Blocks.Add(msgTable);
                    msgTable.CellSpacing = 10;
                    msgTable.Background = Brushes.White;
                    msgTable.RowGroups.Add(new TableRowGroup());
                    TX = msgTable.RowGroups[0];
                }
                TableRow msgStartRow = new TableRow();
                TableRow msgContentRow = new TableRow();


                //TableCell nameCell = new TableCell(new Paragraph(new Run(Name)));
                TableCell TimestampAndNameCell = new TableCell(new Paragraph(new Run("[" + DateTime.Now.ToString() + "]<" + Name + ">")));
                TableCell msgCell = new TableCell(new Paragraph(new Run(Message)));
                //msgCell.ColumnSpan = 2;
                switch (MsgType)
                {
                    case Data.Type.Direct:
                        TimestampAndNameCell.Foreground = Brushes.Chocolate;
                        msgStartRow.Background = Brushes.Honeydew;
                        msgContentRow.Background = Brushes.Honeydew;
                        TimestampAndNameCell.BorderThickness = new Thickness(1);
                        TimestampAndNameCell.BorderBrush = Brushes.Violet;
                        msgCell.BorderThickness = new Thickness(1);
                        msgCell.BorderBrush = Brushes.Violet;
                        break;
                    case Data.Type.Broadcast:
                        TimestampAndNameCell.Foreground = Brushes.DarkGreen;
                        break;
                    case Data.Type.Error:
                        TimestampAndNameCell.Foreground = Brushes.DarkRed;
                        msgCell.Foreground = Brushes.Red;
                        break;
                    case Data.Type.Connect:
                        TimestampAndNameCell.Foreground = Brushes.DarkGoldenrod;
                        break;
                    case Data.Type.Disconnect:
                        TimestampAndNameCell.Foreground = Brushes.DarkBlue;
                        break;
                    default:
                        break;
                }



                //msgStartRow.Cells.Add(nameCell);
                msgStartRow.Cells.Add(TimestampAndNameCell);
                msgContentRow.Cells.Add(msgCell);

                TX.Rows.Add(msgStartRow);
                TX.Rows.Add(msgContentRow);
                rtbMessages.ScrollToEnd();
            });



        }
    }

    public static class JSONExtns
    {

        public static bool IsValidJson(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
