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
using Acurus.Capella.DataAccess.ManagerObjects;
using Acurus.Capella.Core.DomainObjects;
using Telerik.Web.UI;
using System.Collections.Generic;

namespace Acurus.Capella.PatientPortal.UserControls
{
    public partial class CustomDLC : System.Web.UI.UserControl
    {


        //private Unit _textboxheight;
        //private Unit _textboxwidth;
        private string _Value;
        private bool _enable = true;
        private int _listboxheight;
        private string _listboxposition;
        private string _listboxtopposition;

        //public Unit TextboxHeight
        //{
        //    get { return _textboxheight; }
        //    set { _textboxheight = value; }
        //}
        //public Unit TextboxWidth
        //{
        //    get { return _textboxwidth; }
        //    set { _textboxwidth = value; }
        //}

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public string ListboxPosition
        {
            get { return _listboxposition; }
            set { _listboxposition = value; }
        }

        public string ListboxTopPosition
        {
            get { return _listboxtopposition; }
            set { _listboxtopposition = value; }
        }

        public bool Enable
        {
            get { return _enable; }


            set
            {
                _enable = value;
                if (_enable == false)
                {
                    pbDropdown.Attributes.Add("disabled", "disabled");
                    pbClear.Attributes.Add("disabled", "disabled");

                    txtDLC.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    pbDropdown.Attributes.Remove("disabled");
                    pbClear.Attributes.Remove("disabled");
                    txtDLC.Attributes.Remove("disabled");

                    pbDropdown.Enabled = true;
                    pbClear.Enabled = true;
                }

            }

        }

        public int ListboxHeight
        {
            get { return _listboxheight; }
            set { _listboxheight = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request["Value"] != null)
            {
                Value = Request["Value"].ToString();
            }
            if (!IsPostBack)
            {
                hdnTextBoxValue.Value = string.Empty;
                if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
                    pbLibrary.ToolTip = "Library";
                else
                    pbLibrary.ToolTip = "";


                pbClear.Attributes.Add("onclick", "return pbClearAll('" + pbClear.ClientID.Replace("_pbClear", "_txtDLC") + "');");
                if (Request["Value"] != null)
                    pbLibrary.Attributes.Add("onclick", "return OpenAddorUpdate('" + Request["Value"].ToString() + "');");
                if (Request["Value"] != null)
                {
                    Value = Request["Value"].ToString();

                    pbDropdown.Attributes.Add("onclick", "return pbDropdownList('" + pbDropdown.ClientID + "," + "list" + Value.Replace(":", "").Replace('"', ' ') + "');");
                    //txtDLC.Attributes.Add("onkeydown", "insertTab(this,event);");
                }
                //txtDLC.Height = TextboxHeight;
                //txtDLC.Width = TextboxWidth;

                //if (((CustomDLC)sender).Value == "CHIEF_COMPLAINTS")
                //    CCNotes = true;



            }

            SetTheUBACForDynamicControls();
        }
        //protected void pbDropdown_Click(object sender, ImageClickEventArgs e)
        //{
        //    RadListBox lst = new RadListBox();
        //    lst.OnClientSelectedIndexChanged = "OnClientSelectedIndexChanged";
        //    if (Value.Contains(":") == true || Value.Contains('"') == true)
        //        Value = Value.Replace(":", "").Replace('"', ' ');
        //    lst.ID = "list" + Value;
        //    if (pbDropdown.ImageUrl == "~/Resources/plus_new.gif")
        //        pbDropdown.ImageUrl = "~/Resources/minus_new.gif";
        //    if (ListboxHeight != 0)
        //        lst.Height = ListboxHeight;
        //    else
        //        lst.Height = 75;
        //    lst.Width = TextboxWidth;
        //    lst.Font.Bold = false;
        //    lst.Style["position"] = "absolute";

        //    if (ListboxPosition == "")
        //        lst.Style["margin-left"] = "3px";
        //    else
        //        lst.Style["margin-left"] = ListboxPosition;
        //    if (ListboxTopPosition != "" && ListboxTopPosition != null)
        //        lst.Style["margin-top"] = "120px";
        //    this.Controls.Add(lst);

        //    if (CCNotes == true)
        //    {
        //        pbDropdown.ImageUrl = "~/Resources/plus_new.gif";
        //        lst.Visible = false;
        //    }
        //    else
        //        lst.Visible = true;


        //    CCNotes = false;
        //    UserLookupManager userMngr = new UserLookupManager();
        //    IList<UserLookup> fieldList = new List<UserLookup>();
        //    if (lst != null)
        //    {
        //        if (Value != null)
        //        {
        //            fieldList = userMngr.GetFieldLookupList(ClientSession.PhysicianId, Value.ToUpper());
        //            lst.Items.Clear();
        //            for (int i = 0; i < fieldList.Count; i++)
        //            {
        //                RadListBoxItem item = new RadListBoxItem();
        //                item.Attributes.Add(fieldList[i].Value, fieldList[i].Description);
        //                item.Text = fieldList[i].Value;
        //                lst.Items.Add(item);


        //            }
        //        }
        //    }

        //    txtDLC.Focus();
        //    txtDLC.SelectionOnFocus = SelectionOnFocus.CaretToEnd;

        //    divLoading.Style.Add("display", "none");
        //}


        public void SetTheUBACForDynamicControls()
        {
            if (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole == "Coder" || ClientSession.UserRole == "Office Manager" || ClientSession.UserRole == "Front Office")
            {

                pbLibrary.Enabled = false;
            }

            else if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
            {
                if (ClientSession.UserPermission != "U")
                {

                    pbLibrary.Enabled = false;
                    pbLibrary.ToolTip = "";
                }

            }


            if (Status.Value == "None")
            {
                pbDropdown.Enabled = false;

            }




            if (Status.Value == "None")
            {
                pbClear.Enabled = false;

            }



        }
        public void GetTheStatusFromClientSession(string PageName)
        {
            Status.Value = string.Empty;
            int ScreenId = 0;

            string[] formname = PageName.Split('/');
            string screenName = formname[1].Substring(0, formname[1].IndexOf('.'));
            var ScnTabCheck = from c in ClientSession.UserPermissionDTO.Scntab where c.SCN_Name.ToUpper() == screenName.ToUpper() select c;
            IList<ScnTab> TempCheckScnTab = ScnTabCheck.ToList<ScnTab>();
            if (TempCheckScnTab.Count != 0)
            {
                ScreenId = TempCheckScnTab[0].SCN_ID;
            }
            var Screen = from s in ClientSession.UserPermissionDTO.Userscntab where s.scn_id == Convert.ToUInt64(ScreenId) select s;
            IList<user_scn_tab> MyScnTab = Screen.ToList<user_scn_tab>();
            var Processscntab = from s in ClientSession.UserPermissionDTO.ProcessScnTabList where s.Scn_ID == Convert.ToInt32(ScreenId) && s.Process_Name.ToUpper() == ClientSession.UserCurrentProcess.ToUpper() select s;
            IList<ProcessScnTab> MyProcessScnTab = Processscntab.ToList<ProcessScnTab>();
            if (MyScnTab.Count > 0)
            {
                ClientSession.UserPermission = MyScnTab[0].Permission;
            }
            else if (MyProcessScnTab.Count > 0)
            {
                ClientSession.UserPermission = "U";
            }
            if (ClientSession.UserPermission == "R")
            {
                Status.Value = "None";

            }
            else
            {

                if (ClientSession.UserRole.ToUpper() != "PHYSICIAN" && ClientSession.UserRole.ToUpper() != "PHYSICIAN ASSISTANT")
                {
                    if (ClientSession.CheckUser == false && ClientSession.UserPermission == "U")
                    {
                        Status.Value = "Restricted";

                    }
                    else
                    {
                        Status.Value = "None";

                    }

                }
                else
                {
                    Status.Value = "All";
                }


            }
            var process = from p in ClientSession.UserPermissionDTO.ProcessScnTabList where p.Process_Name == ClientSession.UserCurrentProcess && p.Scn_ID == ScreenId select p;

            if (process.ToList<ProcessScnTab>().Count == 0)
            {
                Status.Value = "None";
            }

        }
        public void SetTheUBACorPBACForHistoryControls(Page form)
        {


            if (ClientSession.UserRole == "Medical Assistant" || ClientSession.UserRole == "Office Manager" || ClientSession.UserRole == "Front Office")
            {

                pbLibrary.Enabled = false;

                EnableOrDisableForHistroy(form, false, ClientSession.UserRole);
            }
            else if (ClientSession.UserRole == "Physician Assistant" || ClientSession.UserRole == "Physician")
            {
                if (ClientSession.UserPermission != "U")
                {

                    pbLibrary.Enabled = false;
                }
                else
                {
                    EnableOrDisableForHistroy(form, true, ClientSession.UserRole);


                }

                if (UIManager.PFSH_OpeingFrom == "Menu")
                {

                    pbLibrary.Enabled = true;
                }
            }
            else if (ClientSession.UserRole == "Coder")
            {


                pbDropdown.Enabled = false;


                pbLibrary.Enabled = false;

                pbClear.Enabled = false;


                EnableOrDisableForHistroy(form, false, ClientSession.UserRole);
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
        public void EnableOrDisableForHistroy(Control form, bool permit, string Role)
        {
            List<Control> availControls = GetControls(form);

            foreach (Control c in availControls)
            {
                if (c is ImageButton)
                {
                    var webControl = c as WebControl;
                    ImageButton pb = (ImageButton)webControl;
                    if (pb.ID.Contains("pbLibrary"))
                    {

                        if (!permit || Role.Trim() == "Coder")
                        {
                            pb.ImageUrl = "~/Resources/Database Disable.png";
                            webControl.Enabled = false;
                        }
                        else
                        {
                            pb.ImageUrl = "~/Resources/Database Inactive.jpg";
                            webControl.Enabled = true;
                        }

                        if (Role.Trim().ToUpper() != "PHYSICIAN")
                        {
                            pb.ImageUrl = "~/Resources/Database Disable.png";
                            webControl.Enabled = false;
                        }
                        else
                        {
                            pb.ImageUrl = "~/Resources/Database Inactive.jpg";
                            webControl.Enabled = true;
                        }

                        if (ClientSession.UserCurrentProcess == "CHECK_OUT")
                        {
                            pb.ImageUrl = "~/Resources/Database Disable.png";
                            webControl.Enabled = false;
                        }
                    }
                    else if (pb.ID.Contains("pbDropdown"))
                    {

                        if (Role.Trim() == "Coder" || ClientSession.UserCurrentProcess == "CHECK_OUT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty))
                        {
                            pb.ImageUrl = "~/Resources/plus_new_disabled.gif";
                            webControl.Enabled = false;
                        }




                    }
                    else if (pb.ID.Contains("pbClear"))
                    {
                        if (Role.Trim() == "Coder" || ClientSession.UserCurrentProcess == "CHECK_OUT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty))
                        {
                            pb.ImageUrl = "~/Resources/close_disabled.png";
                            webControl.Enabled = false;
                        }

                    }
                    else if (pb.ID.Contains("RadButton"))
                    {
                        if (Role.Trim() == "Coder" || ClientSession.UserCurrentProcess == "CHECK_OUT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty))
                        {
                            //Bugfix: image issue in Hosp History
                            pb.ImageUrl = "~/Resources/calenda2_Disabled.bmp";
                            webControl.Enabled = false;
                        }
                    }

                }
                else if ((Role.Trim() == "Coder" || ClientSession.UserCurrentProcess == "CHECK_OUT" || (ClientSession.UserCurrentProcess.Trim() == string.Empty && ClientSession.UserCurrentOwner.Trim() == string.Empty)) && (c is RadTextBox || c is RadComboBox || c is CheckBox || c is CheckBoxList || c is ListBox || c is ListView || c is RadDateTimePicker || c is RadDatePicker || c is RadButton || c is Button || c is Panel))
                {

                    var webControl = c as WebControl;
                    webControl.Enabled = false;
                }
                else if (Role.Trim() == "Coder" && c is RadGrid)
                {

                }

            }
        }

        protected void pbDropdown_Click1(object sender, EventArgs e)
        {
            if (hdnTextBoxValue.Value != string.Empty)
            {
                txtDLC.Text = hdnTextBoxValue.Value;
            }
            RadListBox lst = new RadListBox();

            lst.OnClientSelectedIndexChanged = "OnClientSelectedIndexChanged";
            if (Value.Contains(":") == true || Value.Contains('"') == true)
                Value = Value.Replace(":", "").Replace('"', ' ');
            lst.ID = "list" + Value;

            if (ListboxHeight != 0)
                lst.Height = ListboxHeight;
            else
                lst.Height = 75;
            lst.Width = txtDLC.Width;
            lst.Font.Bold = false;
            lst.Style["position"] = "absolute";

            if (ListboxPosition == "")
                lst.Style["margin-left"] = "3px";
            else
                lst.Style["margin-left"] = ListboxPosition;
            if (ListboxTopPosition != "" && ListboxTopPosition != null)
                lst.Style["margin-top"] = "120px";
            this.Controls.Add(lst);

            if (pbDropdown.Text != "-")
            {               
                lst.Visible = true;
                pbDropdown.Text = "-";
            }
        




            UserLookupManager userMngr = new UserLookupManager();
            IList<UserLookup> fieldList = new List<UserLookup>();
            if (lst != null)
            {
                if (Value != null)
                {
                    fieldList = userMngr.GetFieldLookupList(ClientSession.PhysicianId, Value.ToUpper());
                    lst.Items.Clear();
                    for (int i = 0; i < fieldList.Count; i++)
                    {
                        RadListBoxItem item = new RadListBoxItem();
                        item.Attributes.Add(fieldList[i].Value, fieldList[i].Description);
                        item.Text = fieldList[i].Value;
                        lst.Items.Add(item);


                    }
                }
            }

            txtDLC.Focus();
            txtDLC.SelectionOnFocus = SelectionOnFocus.CaretToEnd;

            divLoading.Style.Add("display", "none");
        }
    }
}