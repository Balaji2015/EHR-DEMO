using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Telerik.Web.UI;

namespace Acurus.Capella.UI.UserControls
{
    public partial class CustomDateTimePicker : System.Web.UI.UserControl
    {
        public bool _enable;

        public bool Enable
        {
            get { return _enable; }
            set
            {
                _enable = value;
                if (_enable == false)
                {
                    cboDate.Enabled = false;
                    cboYear.Enabled = false;
                    cboMonth.Enabled = false;
                    RadButton1.Enabled = false;
                    RadButton1.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                }
                else
                {
                    cboDate.Enabled = true;
                    cboYear.Enabled = true;
                    cboMonth.Enabled = true;
                    RadButton1.Enabled = true;
                    RadButton1.ImageUrl = "~/Resources/calenda2.bmp";
                    _enable = value;
                }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ClientSession.LocalDate != null && ClientSession.LocalDate != string.Empty)
                {
                    clbCalendar.FocusedDate = Convert.ToDateTime(ClientSession.LocalDate);
                    clbCalendar.RangeMaxDate = Convert.ToDateTime(ClientSession.LocalDate);
                    clbCalendar.SelectedDate = Convert.ToDateTime(ClientSession.LocalDate);
                }
                RadButton1.Attributes.Add("onclick", "return OnCustomDTPClick('" + clbCalendar.ClientID + "," + cboYear.ClientID + "," + cboMonth.ClientID + "," + cboDate.ClientID + "');");
            }
            else
            {
                RadButton1.Attributes.Add("onclick", "return OnCustomDTPClick('" + clbCalendar.ClientID + "," + cboYear.ClientID + "," + cboMonth.ClientID + "," + cboDate.ClientID + "');");
            }

        }





        public void AssignDate(string sYear, string sMonth, string sDate)
        {
            cboYear.Text = sYear;
            cboMonth.Text = sMonth;
            cboDate.Text = sDate;

            DateTime month = Convert.ToDateTime("1/1/2000");

            cboMonth.Items.Clear();
            cboMonth.Items.Add(new RadComboBoxItem(""));

            for (int i = 0; i < 12; i++)
            {
                DateTime NextMont = month.AddMonths(i);
                RadComboBoxItem tempItem = new RadComboBoxItem();
                tempItem.Text = NextMont.ToString("MMM");
                tempItem.Value = i.ToString();
                cboMonth.Items.Add(tempItem);
            }
            cboYear.Items.Clear();
            cboYear.Items.Add(new RadComboBoxItem(""));
            for (int i = DateTime.Now.Year; i >= 1900; i--)
            {
                RadComboBoxItem tempItem = new RadComboBoxItem();
                tempItem.Text = i.ToString();
                cboYear.Items.Add(tempItem);
            }

            cboDate.Items.Add(new RadComboBoxItem(""));
            for (int i = 1; i <= 31; i++)
            {
                RadComboBoxItem tempItem = new RadComboBoxItem();
                tempItem.Text = i.ToString();
                cboDate.Items.Add(tempItem);

            }

        }

        protected void Label1_Load(object sender, EventArgs e)
        {
            (sender as Label).Text = DateTime.Now.ToString();
        }
        protected void Button1_Load(object sender, EventArgs e)
        {
            (sender as Button).Attributes.Add("onclick", "return doStuff('" + clbCalendar.ClientID + "')");
        }
        public void SetTheUBACorPBACForHistoryControls(Page form)
        {
            EnableOrDisableForHistroy(form, false, ClientSession.UserRole);
        }

        public void EnableOrDisableForHistroy(Control form, bool permit, string Role)
        {
            List<Control> availControls = GetControls(form);

            foreach (Control c in availControls)
            {
                if ((Role.Trim() == "Coder" || ClientSession.UserCurrentProcess == "CHECK_OUT" || ClientSession.UserCurrentProcess == "CHECK_OUT_WAIT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty)) && (c is RadTextBox || c is RadComboBox || c is CheckBox || c is CheckBoxList || c is ListBox || c is ListView || c is RadDateTimePicker || c is RadDatePicker || c is RadButton || c is Button || c is Panel))
                {
                    var webControl = c as WebControl;
                    webControl.Enabled = false;

                    if (c is HtmlInputCheckBox)
                    {
                        var webControlHTML = c as HtmlInputCheckBox;
                        webControlHTML.Disabled = true;
                    }
                    else if (c is HtmlInputButton)
                    {
                        var webControlHTMLButton = c as HtmlInputButton;
                        webControlHTMLButton.Disabled = true;
                    }

                }
                if (Role.Trim() == "Coder" || ClientSession.UserCurrentProcess == "MA_PROCESS")
                {
                    if (c is Button && ((Button)c).Text == "L")
                    {
                        var webControl = c as WebControl;
                        webControl.Enabled = false;
                    }
                }
            }
        }
        private static List<Control> GetControls(Control form)
        {
            var controlList = new List<Control>();

            foreach (Control childControl in form.Controls)
            {
                controlList.AddRange(GetControls(childControl));
                controlList.Add(childControl);
            }
            return controlList;
        }

    }


}