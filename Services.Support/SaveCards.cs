using De.HsFlensburg.ClientApp101.Business.Model.BusinessObjects;
using De.HsFlensburg.ClientApp101.Logic.Ui.ViewModels;
using De.HsFlensburg.ClientApp101.Logic.Ui.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;


namespace Services.Support
{
    class SaveCards
    {

        static void SaveCardsToFile(BoxViewModel box)
        {
            System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App");
            System.IO.Directory.CreateDirectory(Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content");
            string saveDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App";
            string pictureDirectory = Environment.SpecialFolder.MyDocuments + @"\Lernkarten-App\content";
            ArrayList categorys = new ArrayList();
            foreach(CardViewModel card in box)
            {
                if (!categorys.Contains(card.Category))
                {
                    categorys.Add(card.Category);
                }
            }
            foreach(var item in categorys)
            {
                MessageBox.Show((string)item);
            }
        }
    }
}
