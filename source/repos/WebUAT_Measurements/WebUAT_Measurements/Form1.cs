using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebUAT_Measurements
{
    public partial class Form1 : Form
    {
        int admin_EditionMode;
        WebUATController controller;
        GroupBox[] adminGroupBoxes;

        //DataRow Admin variables
        DataRow currentDataRow;
        int dataRowNewPart;

        //Template Admin variables
        Template currentTemplate;

        public Form1()
        {
            InitializeComponent();
            admin_EditionMode = MyronWebUAT.admin_editmode_none;
            adminGroupBoxes = new GroupBox[5] { Grp_DataRows, Grp_StdTranslations, Grp_FixedTranslations, Grp_ConditionalTranslations, Grp_Template };

            currentDataRow = null;
            currentTemplate = null;
            dataRowNewPart = MyronWebUAT.admin_notSelected;

            // Load WebUATController data from Json file. If file doesn't exist it will create a new one from an empty instance.
            controller = WebUATController.LoadWebUATJsonData();
        }

        private void Cmb_admin_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillAdminList(Cmb_Admin_ContentType.SelectedIndex);
        }

        private void FillAdminList(int contentType)
        {
            //Clear the list first
            List_Admin_Items.Items.Clear();
            //Fill it with the right content
            if (contentType == MyronWebUAT.admin_contenttype_dataRows)
            {
                for (int i = 0; i < controller.dataRows.Count; i++)
                {
                    //The row description will be displayed on the list
                    List_Admin_Items.Items.Add(controller.dataRows[i].description);
                }
            }
            else if (contentType == MyronWebUAT.admin_contenttype_standardTranslations)
            {
                for (int i = 0; i < controller.standardTranslations.Count; i++)
                {
                    //Add the eng_MY as the default language to be displayed on the list
                    List_Admin_Items.Items.Add(controller.standardTranslations[i].eng_MY);
                }
            }
            else if (contentType == MyronWebUAT.admin_contenttype_valuedFixedTranslations)
            {
                for (int i = 0; i < controller.fixedTranslations.Count; i++)
                {
                    //Add the Value description to be displayed on the list
                    List_Admin_Items.Items.Add(controller.fixedTranslations[i].valueDescription);
                }
            }
            else if (contentType == MyronWebUAT.admin_contenttype_valuedConditionedTranslations)
            {
                for (int i = 0; i < controller.conditionedTranslations.Count; i++)
                {
                    //Add the Value description to be displayed on the list
                    List_Admin_Items.Items.Add(controller.conditionedTranslations[i].valueDescription);
                }
            }
            else if (contentType == MyronWebUAT.admin_contenttype_templates)
            {
                for (int i = 0; i < controller.templates.Count; i++)
                {
                    //Add the template Name to be displayed on the list
                    List_Admin_Items.Items.Add(controller.templates[i].templateName);
                }
            }

            List_Admin_Items.Refresh();
        }

        /// <summary>
        /// This method enables the admin buttons (New, Edit, Save). For this to be effective, the default tab when the app 
        /// opens must be Measurements.
        /// </summary>
        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Btn_Admin_Cancel.PerformClick();

            //If the Tab is changed to other that is not admin, set the edition mode to none to prevent undesired changes if save is clicked later
            if (TabControl.SelectedIndex != MyronWebUAT.tab_admin)
            {
                admin_EditionMode = MyronWebUAT.admin_editmode_none;
            }
        }

        private void Btn_Admin_New_Click(object sender, EventArgs e)
        {
            if (Cmb_Admin_ContentType.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                admin_EditionMode = MyronWebUAT.admin_editmode_new;
                ToggleEditorGroups(Cmb_Admin_ContentType.SelectedIndex);
                List_Admin_Items.SelectedIndex = MyronWebUAT.admin_notSelected;

                //Lock elements that can cause problems and enable Cancel button
                Cmb_Admin_ContentType.Enabled = false;
                List_Admin_Items.Enabled = false;
                Btn_Admin_Delete.Enabled = false;
                Btn_Admin_Edit.Enabled = false;
                Btn_Admin_Cancel.Enabled = true;

                //If dataRow, instantiate the new dataRow
                if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_dataRows)
                {
                    currentDataRow = new DataRow();
                    dataRowNewPart = MyronWebUAT.admin_notSelected;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_templates)
                {
                    Radio_Temp_ItemSize.Checked = true;
                    Cmb_Temp_DataRows.Items.Clear();
                    List_Temp_ItemSize.Items.Clear();
                    List_Temp_ImprintArea.Items.Clear();
                    List_Temp_Charges.Items.Clear();

                    currentTemplate = new Template();
                    for (int i = 0; i < controller.dataRows.Count; i++)
                    {
                        Cmb_Temp_DataRows.Items.Add(controller.dataRows[i].description);
                    }
                }
            }
        }

        /// <summary>
        /// Makes visible the group that matches the contentType and hide the others. NOTE: To hide all groups 
        /// pass admin_contentType_notSelected as param.
        /// </summary>
        private void ToggleEditorGroups(int contentType)
        {
            for (int i = 0; i < adminGroupBoxes.Length; i++)
            {
                if (i == contentType)
                {
                    adminGroupBoxes[i].Visible = true;
                }
                else
                {
                    adminGroupBoxes[i].Visible = false;
                }
            }
            //Also clear all text fields by setting MY and replicating to all translation fields
            ClearEditorGroupsData();
        }

        /// <summary>
        /// This function clears all the fields in all groups.
        /// </summary>
        private void ClearEditorGroupsData()
        {
            //Clear std translations 
            Text_TextOnly_EngMY.Text = "";
            Btn_TextOnly_ReplicateMY.PerformClick();
            //Clear fixed translations
            Text_Fixed_ValueDescription.Text = "";
            Text_Fixed_MY.Text = "";
            Btn_Fixed_ReplicateMY.PerformClick();
            Cmb_Fixed_ValueType.SelectedIndex = MyronWebUAT.admin_notSelected;
            //Clear conditional translations
            Text_Cdtl_MY.Text = "";
            Btn_Cdtl_ReplicateMY.PerformClick();
            Text_Cdtl_ElseMY.Text = "";
            Btn_Cdtl_ELSE_ReplicateMY.PerformClick();
            Text_Cdtl_ValueDescriptor.Text = "";
            Chck_Cdtl_UsesVariable.Checked = false;
            Text_Cdtl_Variable.Text = "";
            //Clear Data Row fields
            Btn_DataRow_Cancel.PerformClick();
            Text_DataRow_Description.Text = "";
            currentDataRow = null;
            dataRowNewPart = MyronWebUAT.admin_notSelected;
            FillDataRowPartsList(null);
            //Clear Template fields
            currentTemplate = null;
            Text_Temp_Description.Text = "";
            Cmb_Temp_DataRows.SelectedIndex = MyronWebUAT.admin_notSelected;
            Cmb_Temp_DataRows.Items.Clear();
            List_Temp_ItemSize.Items.Clear();
            List_Temp_ImprintArea.Items.Clear();
            List_Temp_Charges.Items.Clear();
            Radio_Temp_ItemSize.Checked = true;
        }

        private void Btn_admin_edit_Click(object sender, EventArgs e)
        {
            if (List_Admin_Items.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                admin_EditionMode = MyronWebUAT.admin_editmode_edit;
                ToggleEditorGroups(Cmb_Admin_ContentType.SelectedIndex);
                //Lock elements that can cause problems and enable Cancel button
                Cmb_Admin_ContentType.Enabled = false;
                List_Admin_Items.Enabled = false;
                Btn_Admin_Delete.Enabled = false;
                Btn_Admin_Edit.Enabled = false;
                Btn_Admin_Cancel.Enabled = true;

                int index = List_Admin_Items.SelectedIndex;

                //Load data into the fields depending on the content type
                if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_dataRows)
                {
                    currentDataRow = controller.dataRows[index];
                    Text_DataRow_Description.Text = currentDataRow.description;
                    List_DataRow_Parts.Items.Clear();
                    for (int i = 0; i < currentDataRow.dataRowParts.Count; i++)
                    {
                        List_DataRow_Parts.Items.Add(controller.GetTranslationDescription(currentDataRow.dataRowParts[i]));
                    }
                    List_DataRow_Parts.Refresh();
                    RefreshFinalDataRowTextBox();
                    HideDataRowCustomTextField();
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedConditionedTranslations)
                {
                    Text_Cdtl_ValueDescriptor.Text = controller.conditionedTranslations[index].valueDescription;
                    Chck_Cdtl_UsesVariable.Checked = controller.conditionedTranslations[index].canUseVariable;
                    Text_Cdtl_Variable.Text = controller.conditionedTranslations[index].variable;
                    Text_Cdtl_MY.Text = controller.conditionedTranslations[index].eng_MY;
                    Text_Cdtl_CAAZ.Text = controller.conditionedTranslations[index].eng_CAAZ;
                    Text_Cdtl_CAFR.Text = controller.conditionedTranslations[index].fr_CA;
                    Cmb_Cdtl_ConditionType.SelectedIndex = controller.conditionedTranslations[index].conditionType;
                    Text_Cdtl_ElseMY.Text = controller.conditionedTranslations[index].elseEng_MY;
                    Text_Cdtl_ElseCAAZ.Text = controller.conditionedTranslations[index].elseEng_CAAZ;
                    Text_Cdtl_ElseFRCA.Text = controller.conditionedTranslations[index].elseFR_CA;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedFixedTranslations)
                {
                    Text_Fixed_ValueDescription.Text = controller.fixedTranslations[index].valueDescription;
                    Text_Fixed_MY.Text = controller.fixedTranslations[index].eng_MY;
                    Text_Fixed_CAAZ.Text = controller.fixedTranslations[index].eng_CAAZ;
                    Text_Fixed_CAFR.Text = controller.fixedTranslations[index].fr_CA;
                    Cmb_Fixed_ValueType.SelectedIndex = controller.fixedTranslations[index].valueType;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_standardTranslations)
                {
                    Text_TextOnly_EngMY.Text = controller.standardTranslations[index].eng_MY;
                    Text_TextOnly_EngCAAZ.Text = controller.standardTranslations[index].eng_CAAZ;
                    Text_TextOnly_FRCA.Text = controller.standardTranslations[index].fr_CA;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_templates)
                {
                    for (int i = 0; i < controller.dataRows.Count; i++)
                    {
                        Cmb_Temp_DataRows.Items.Add(controller.dataRows[i].description);
                    }
                    currentTemplate = controller.templates[index];
                    Text_Temp_Description.Text = currentTemplate.templateName;
                    RefreshTemplateItemSizeList();
                    RefreshTemplateImprintAreaList();
                    RefreshTemplateChargesList();

                    Radio_Temp_ItemSize.Checked = true;
                }
            }
        }

        /// <summary>
        /// Called when Save button in Admin is called. It will internally evaluate if it's gonna save a new translation or an edition.
        /// </summary>
        private void Btn_Admin_Save_Click(object sender, EventArgs e)
        {
            bool saveToFile = false;
            if (admin_EditionMode == MyronWebUAT.admin_editmode_new)
            {
                if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_dataRows && ValidateDataRow())
                {
                    currentDataRow.description = Text_DataRow_Description.Text;
                    controller.dataRows.Add(currentDataRow);
                    //The newDataRow variable is cleaned after save on ToggleEditorGroups call
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_standardTranslations && ValidateStandardTranslations())
                {
                    Translation stdTranslation = new Translation(Text_TextOnly_EngMY.Text, Text_TextOnly_EngCAAZ.Text, Text_TextOnly_FRCA.Text);
                    controller.standardTranslations.Add(stdTranslation);
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedFixedTranslations && ValidateFixedTranslations())
                {
                    ValuedFixedTranslation fixedTranslation = new ValuedFixedTranslation(Text_Fixed_MY.Text, Text_Fixed_CAAZ.Text,
                        Text_Fixed_CAFR.Text, Cmb_Fixed_ValueType.SelectedIndex, Text_Fixed_ValueDescription.Text, Chck_Fixed_CanUseVariable.Checked, Text_Fixed_Variable.Text);
                    controller.fixedTranslations.Add(fixedTranslation);
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedConditionedTranslations && ValidateConditionedTranslations())
                {
                    ValuedConditionedTranslation conditionedTranslation = new ValuedConditionedTranslation(Text_Cdtl_MY.Text, Text_Cdtl_CAAZ.Text, Text_Cdtl_CAFR.Text,
                        Text_Cdtl_ValueDescriptor.Text, Chck_Cdtl_UsesVariable.Checked, Text_Cdtl_Variable.Text, Cmb_Cdtl_ConditionType.SelectedIndex,
                        Text_Cdtl_ElseMY.Text, Text_Cdtl_ElseCAAZ.Text, Text_Cdtl_ElseFRCA.Text);
                    controller.conditionedTranslations.Add(conditionedTranslation);
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_templates && ValidateTemplate())
                {
                    currentTemplate.templateName = Text_Temp_Description.Text;
                    controller.templates.Add(currentTemplate);
                    //The currentTemplate variable is cleaned after save on ToggleEditorGroups call
                    saveToFile = true;
                }
            }
            else if (admin_EditionMode == MyronWebUAT.admin_editmode_edit)
            {
                if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_dataRows && ValidateDataRow())
                {
                    controller.dataRows[List_Admin_Items.SelectedIndex] = currentDataRow;
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_standardTranslations && ValidateStandardTranslations())
                {
                    controller.standardTranslations[List_Admin_Items.SelectedIndex].FillTranslation(Text_TextOnly_EngMY.Text, Text_TextOnly_EngCAAZ.Text, Text_TextOnly_FRCA.Text);
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedFixedTranslations && ValidateFixedTranslations())
                {
                    controller.fixedTranslations[List_Admin_Items.SelectedIndex].FillValuedFixedTranslation(Text_Fixed_MY.Text, Text_Fixed_CAAZ.Text,
                        Text_Fixed_CAFR.Text, Cmb_Fixed_ValueType.SelectedIndex, Text_Fixed_ValueDescription.Text, Chck_Fixed_CanUseVariable.Checked, Text_Fixed_Variable.Text);
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedConditionedTranslations && ValidateConditionedTranslations())
                {
                    controller.conditionedTranslations[List_Admin_Items.SelectedIndex].FillValuedConditionedTranslation(Text_Cdtl_MY.Text, Text_Cdtl_CAAZ.Text, Text_Cdtl_CAFR.Text,
                        Text_Cdtl_ValueDescriptor.Text, Chck_Cdtl_UsesVariable.Checked, Text_Cdtl_Variable.Text, Cmb_Cdtl_ConditionType.SelectedIndex,
                        Text_Cdtl_ElseMY.Text, Text_Cdtl_ElseCAAZ.Text, Text_Cdtl_ElseFRCA.Text);
                    saveToFile = true;
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_templates && ValidateTemplate())
                {
                    currentTemplate.templateName = Text_Temp_Description.Text;
                    controller.templates[List_Admin_Items.SelectedIndex] = currentTemplate;
                    saveToFile = true;
                }
            }

            if (saveToFile)
            {
                controller.SaveWebUATJsonData();
                FillAdminList(Cmb_Admin_ContentType.SelectedIndex);
                ToggleEditorGroups(MyronWebUAT.admin_notSelected);
                //Clicking cancel restore all the buttons to the normal status
                Btn_Admin_Cancel.PerformClick();
            }
        }

        public bool ValidateTemplate()
        {
            return Text_Temp_Description.Text.Length > 0 && (currentTemplate.itemSizeDataRowList.Count > 0
                || currentTemplate.imprintAreaDataRowList.Count > 0 || currentTemplate.chargesDataRowList.Count > 0);
        }

        public bool ValidateDataRow()
        {
            return Text_DataRow_Description.Text.Length > 0 && List_DataRow_Parts.Items.Count > 0;
        }

        /// <summary>
        /// Validates if the valued field is valid: at least must contain the value token. TO BE TESTED!
        /// </summary>
        public bool ValidateValuedTranslationField(string field)
        {
            //Field must at least contain "{x}" which are 3 characters
            return (field.Length >= 2 && 
                field.IndexOf(MyronWebUAT.valueToken) != -1 &&
                field.IndexOf(MyronWebUAT.valueToken) == field.LastIndexOf(MyronWebUAT.valueToken));
        }

        /// <summary>
        /// Checks that the selected valueType is valid value.
        /// </summary>
        public bool ValidateSelectedValueType(int selected)
        {
            return selected == MyronWebUAT.valuetype_fraction || selected == MyronWebUAT.valuetype_integer || selected == MyronWebUAT.valuetype_translation;
        }

        /// <summary>
        /// Checks that the selected conditionType is valid value.
        /// </summary>
        public bool ValidateConditionType(int selected)
        {
            return selected == MyronWebUAT.condtiontype_qtyEqual1 || selected == MyronWebUAT.conditiontype_lastValueOnDataRow;
        }

        /// <summary>
        /// Method validates all the input fields for VAlued Fixed Translation before saving it.
        /// </summary>
        public bool ValidateFixedTranslations()
        {
            //Check the conditions on all controls. Translations must be > 3 at least because it must include {x}
            return Text_Fixed_ValueDescription.Text != "" && ValidateValuedTranslationField(Text_Fixed_MY.Text)
                && ValidateValuedTranslationField(Text_Fixed_CAFR.Text) && ValidateValuedTranslationField(Text_Fixed_CAAZ.Text)
                && ValidateSelectedValueType(Cmb_Fixed_ValueType.SelectedIndex);
        }

        /// <summary>
        /// Method validates all the input fields for Valued Conditioned Translation before saving it.
        /// </summary>
        public bool ValidateConditionedTranslations()
        {
            //Check the conditions on all controls. Translations must be > 3 at least because it must include {x}
            return Text_Cdtl_ValueDescriptor.Text != "" && ValidateValuedTranslationField(Text_Cdtl_MY.Text)
                && ValidateValuedTranslationField(Text_Cdtl_CAAZ.Text) && ValidateValuedTranslationField(Text_Cdtl_CAFR.Text)
                && ValidateConditionType(Cmb_Cdtl_ConditionType.SelectedIndex) && ValidateValuedTranslationField(Text_Cdtl_ElseMY.Text)
                && ValidateValuedTranslationField(Text_Cdtl_ElseCAAZ.Text) && ValidateValuedTranslationField(Text_Cdtl_ElseFRCA.Text);
        }

        /// <summary>
        /// Method validates all the input fields for Standard Translation before saving it.
        /// </summary>
        public bool ValidateStandardTranslations()
        {
            //Check the conditions on all controls. Translations fields must not be emtpy.
            return Text_TextOnly_EngMY.Text.Length > 0 && Text_TextOnly_EngCAAZ.Text.Length > 0 && Text_TextOnly_FRCA.Text.Length > 0;
        }

        private void Btn_TextOnly_ReplicateMY_Click(object sender, EventArgs e)
        {
            Text_TextOnly_EngCAAZ.Text = Text_TextOnly_EngMY.Text;
            Text_TextOnly_FRCA.Text = Text_TextOnly_EngMY.Text;
        }

        private void Btn_Cdtl_ReplicateMY_Click(object sender, EventArgs e)
        {
            Text_Cdtl_CAAZ.Text = Text_Cdtl_MY.Text;
            Text_Cdtl_CAFR.Text = Text_Cdtl_MY.Text;
        }

        private void Btn_Cdtl_ELSE_ReplicateMY_Click(object sender, EventArgs e)
        {
            Text_Cdtl_ElseCAAZ.Text = Text_Cdtl_ElseMY.Text;
            Text_Cdtl_ElseFRCA.Text = Text_Cdtl_ElseMY.Text;
        }

        private void Btn_ReplicateMY_Click(object sender, EventArgs e)
        {
            Text_Fixed_CAAZ.Text = Text_Fixed_MY.Text;
            Text_Fixed_CAFR.Text = Text_Fixed_MY.Text;
        }

        private void Btn_Admin_Cancel_Click(object sender, EventArgs e)
        {
            Cmb_Admin_ContentType.Enabled = true;
            List_Admin_Items.Enabled = true;
            Btn_Admin_Delete.Enabled = true;
            Btn_Admin_Edit.Enabled = true;
            Btn_Admin_Cancel.Enabled = false;
            ToggleEditorGroups(MyronWebUAT.admin_notSelected);
            ClearEditorGroupsData();
        }

        /// <summary>
        /// Remove the selected element in the list.
        /// </summary>
        private void Btn_Admin_Delete_Click(object sender, EventArgs e)
        {
            if (List_Admin_Items.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                int indexToRemove = List_Admin_Items.SelectedIndex;

                if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_dataRows)
                {

                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_standardTranslations)
                {
                    controller.standardTranslations.RemoveAt(indexToRemove);
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedFixedTranslations)
                {
                    controller.fixedTranslations.RemoveAt(indexToRemove);
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_valuedConditionedTranslations)
                {
                    controller.conditionedTranslations.RemoveAt(indexToRemove);
                }
                else if (Cmb_Admin_ContentType.SelectedIndex == MyronWebUAT.admin_contenttype_templates)
                {
                    controller.templates.RemoveAt(indexToRemove);
                }

                controller.SaveWebUATJsonData();
                FillAdminList(Cmb_Admin_ContentType.SelectedIndex);
                ToggleEditorGroups(MyronWebUAT.admin_notSelected);
                //Clicking cancel restore all the buttons to the normal status
                Btn_Admin_Cancel.PerformClick();
            }
        }

        private void Btn_DataRow_AddCustomText_Click(object sender, EventArgs e)
        {
            //Make sure the cmb for translations is hidden and clear
            Cmb_DataRow_Translations.Visible = false;
            Lbl_DataRow_SelectTranslation.Visible = false;
            Cmb_DataRow_Translations.Items.Clear();
            //Make visible the custom text Text field
            Text_DataRow_CustomText.Text = "";
            Text_DataRow_CustomText.Visible = true;
            Lbl_DataRow_CustomText.Visible = true;
            //Flag the newPart type
            dataRowNewPart = MyronWebUAT.dataType_customText;

        }

        private void Btn_DataRow_AddStdTranslation_Click(object sender, EventArgs e)
        {
            HideDataRowCustomTextField();
            //Clean the translations combo
            Cmb_DataRow_Translations.Items.Clear();
            //Add all the items from controller.std translations
            for (int i = 0; i < controller.standardTranslations.Count; i++)
            {
                Cmb_DataRow_Translations.Items.Add(controller.standardTranslations[i].eng_MY);
            }
            //Make the Cmb visilble and its label
            Cmb_DataRow_Translations.Visible = true;
            Lbl_DataRow_SelectTranslation.Visible = true;
            //Flag the dataRowNewPart
            dataRowNewPart = MyronWebUAT.dataType_stdTranslation;
        }

        private void HideDataRowCustomTextField()
        {
            //Hide the custom text Text field
            Text_DataRow_CustomText.Text = "";
            Text_DataRow_CustomText.Visible = false;
            Lbl_DataRow_CustomText.Visible = false;
        }

        private void Btn_DataRow_AddFixedTranslation_Click(object sender, EventArgs e)
        {
            HideDataRowCustomTextField();
            //Clean the translations combo
            Cmb_DataRow_Translations.Items.Clear();
            //Add all the items from controller.std translations
            for (int i = 0; i < controller.fixedTranslations.Count; i++)
            {
                Cmb_DataRow_Translations.Items.Add(controller.fixedTranslations[i].valueDescription);
            }
            //Make the Cmb visilble and its label
            Cmb_DataRow_Translations.Visible = true;
            Lbl_DataRow_SelectTranslation.Visible = true;

            //Flag the dataRowNewPart
            dataRowNewPart = MyronWebUAT.dataType_fixedTranslation;
        }

        private void Btn_DataRow_AddCdtlTranslation_Click(object sender, EventArgs e)
        {
            HideDataRowCustomTextField();
            //Clean the translations combo
            Cmb_DataRow_Translations.Items.Clear();
            //Add all the items from controller.std translations
            for (int i = 0; i < controller.conditionedTranslations.Count; i++)
            {
                Cmb_DataRow_Translations.Items.Add(controller.conditionedTranslations[i].valueDescription);
            }
            //Make the Cmb visilble and its label
            Cmb_DataRow_Translations.Visible = true;
            Lbl_DataRow_SelectTranslation.Visible = true;

            //Flag the dataRowNewPart
            dataRowNewPart = MyronWebUAT.dataType_conditionedTranslation;
        }

        private void RefreshFinalDataRowTextBox()
        {
            //Clean it
            Text_DataRow_FinalResult.Text = "";
            //Fill again, check wich translations to use
            if (currentDataRow != null)
            {
                if (Radio_DataRow_EngMY.Checked)
                {
                    for (int i = 0; i < currentDataRow.dataRowParts.Count; i++)
                    {
                        Text_DataRow_FinalResult.Text += controller.FindTranslationById(currentDataRow.dataRowParts[i]).eng_MY;
                    }
                }
                else if (Radio_DataRow_EngAZ.Checked)
                {
                    for (int i = 0; i < currentDataRow.dataRowParts.Count; i++)
                    {
                        Text_DataRow_FinalResult.Text += controller.FindTranslationById(currentDataRow.dataRowParts[i]).eng_CAAZ;
                    }
                }
                else if (Radio_DataRow_FrCA.Checked)
                {
                    for (int i = 0; i < currentDataRow.dataRowParts.Count; i++)
                    {
                        Text_DataRow_FinalResult.Text += controller.FindTranslationById(currentDataRow.dataRowParts[i]).fr_CA;
                    }
                }
            }
        }

        private void Btn_DataRow_Add_Click(object sender, EventArgs e)
        {
            if (dataRowNewPart == MyronWebUAT.dataType_customText && Text_DataRow_CustomText.Text != "")
            {
                Translation customText = new Translation(MyronWebUAT.dataType_customText, Text_DataRow_CustomText.Text, Text_DataRow_CustomText.Text, Text_DataRow_CustomText.Text);
                //Save the new customText in the controller
                controller.customTextTranslations.Add(customText);
                //New Custom text created and added to the datarow
                currentDataRow.dataRowParts.Add(customText.translationID);
                //Update the list
                List_DataRow_Parts.Items.Add(customText.eng_MY);
                List_DataRow_Parts.Refresh();
                RefreshFinalDataRowTextBox();
                HideDataRowCustomTextField();
            }
            else if (dataRowNewPart == MyronWebUAT.dataType_stdTranslation && Cmb_DataRow_Translations.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                Translation stdTranslation = controller.standardTranslations[Cmb_DataRow_Translations.SelectedIndex];
                //Std translation added to the datarow
                currentDataRow.dataRowParts.Add(stdTranslation.translationID);
                //Update the list
                List_DataRow_Parts.Items.Add(stdTranslation.eng_MY);
                List_DataRow_Parts.Refresh();
                RefreshFinalDataRowTextBox();
                //Make the Cmb visilble and its label
                Cmb_DataRow_Translations.Visible = false;
                Lbl_DataRow_SelectTranslation.Visible = false;
                Cmb_DataRow_Translations.Items.Clear();
            }
            else if (dataRowNewPart == MyronWebUAT.dataType_fixedTranslation && Cmb_DataRow_Translations.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                ValuedFixedTranslation fixedTranslation = controller.fixedTranslations[Cmb_DataRow_Translations.SelectedIndex];
                //Fixed translation added to the datarow
                currentDataRow.dataRowParts.Add(fixedTranslation.translationID);
                //Update the list with the description
                List_DataRow_Parts.Items.Add(fixedTranslation.valueDescription);
                List_DataRow_Parts.Refresh();
                RefreshFinalDataRowTextBox();
                //Make the Cmb visilble and its label
                Cmb_DataRow_Translations.Visible = false;
                Lbl_DataRow_SelectTranslation.Visible = false;
                Cmb_DataRow_Translations.Items.Clear();
            }
            else if (dataRowNewPart == MyronWebUAT.dataType_conditionedTranslation && Cmb_DataRow_Translations.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                ValuedConditionedTranslation conditionedTranslation = controller.conditionedTranslations[Cmb_DataRow_Translations.SelectedIndex];
                //Conditioned translation added to the datarow
                currentDataRow.dataRowParts.Add(conditionedTranslation.translationID);
                //Update the list with the description
                List_DataRow_Parts.Items.Add(conditionedTranslation.valueDescription);
                List_DataRow_Parts.Refresh();
                RefreshFinalDataRowTextBox();
                //Make the Cmb visilble and its label
                Cmb_DataRow_Translations.Visible = false;
                Lbl_DataRow_SelectTranslation.Visible = false;
                Cmb_DataRow_Translations.Items.Clear();
            }

            Text_DataRow_CustomText.Text = "";
            Btn_DataRow_Cancel.PerformClick();
        }

        private void Radio_DataRow_EngAZ_CheckedChanged(object sender, EventArgs e)
        {
            RefreshFinalDataRowTextBox();
        }

        private void Radio_DataRow_FrCA_CheckedChanged(object sender, EventArgs e)
        {
            RefreshFinalDataRowTextBox();
        }

        private void Radio_DataRow_EngMY_CheckedChanged(object sender, EventArgs e)
        {
            RefreshFinalDataRowTextBox();
        }

        private void Btn_DataRow_Delete_Click(object sender, EventArgs e)
        {
            if (List_DataRow_Parts.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                Btn_DataRow_Cancel.PerformClick();
                currentDataRow.dataRowParts.RemoveAt(List_DataRow_Parts.SelectedIndex);
                //Refresh both List and Final Data Textbox
                FillDataRowPartsList(currentDataRow.dataRowParts);
            }

        }

        private void FillDataRowPartsList(List<string> dataRowParts)
        {
            List_DataRow_Parts.Items.Clear();
            if (currentDataRow != null)
            {
                Translation translation = null;
                foreach (string translationID in dataRowParts)
                {
                    translation = controller.FindTranslationById(translationID);

                    //If it's a custom txt || std translation, add the Engl. translation
                    if (translationID.Contains(MyronWebUAT.dataType_customText.ToString() + "_") || translationID.Contains(MyronWebUAT.dataType_stdTranslation.ToString() + "_"))
                    {
                        List_DataRow_Parts.Items.Add(translation.eng_MY);
                    }
                    //If it's a fixed translation, add the description
                    else if (translationID.Contains(MyronWebUAT.dataType_fixedTranslation.ToString() + "_"))
                    {
                        List_DataRow_Parts.Items.Add(((ValuedFixedTranslation)translation).valueDescription);
                    }
                    //If it's a conditioned translation, add the description
                    else if (translationID.Contains(MyronWebUAT.dataType_fixedTranslation.ToString() + "_"))
                    {
                        List_DataRow_Parts.Items.Add(((ValuedConditionedTranslation)translation).valueDescription);
                    }
                }

            }

            List_DataRow_Parts.Refresh();
            RefreshFinalDataRowTextBox();
        }

        private void Btn_DataRow_Cancel_Click(object sender, EventArgs e)
        {
            HideDataRowCustomTextField();
            Lbl_DataRow_SelectTranslation.Visible = false;
            Cmb_DataRow_Translations.Visible = false;
            Cmb_DataRow_Translations.SelectedIndex = MyronWebUAT.admin_notSelected;
            Cmb_DataRow_Translations.Items.Clear();
            dataRowNewPart = MyronWebUAT.admin_notSelected;
        }

        private void Btn_DataRow_MoveUp_Click(object sender, EventArgs e)
        {
            if (List_DataRow_Parts.SelectedIndex > 0 && currentDataRow != null)
            {
                int newPosition = List_DataRow_Parts.SelectedIndex - 1;
                string temp = currentDataRow.dataRowParts[newPosition];
                currentDataRow.dataRowParts[newPosition] = currentDataRow.dataRowParts[List_DataRow_Parts.SelectedIndex];
                currentDataRow.dataRowParts[List_DataRow_Parts.SelectedIndex] = temp;
                FillDataRowPartsList(currentDataRow.dataRowParts);
                List_DataRow_Parts.SelectedIndex = newPosition;

            }
        }

        private void Btn_DataRow_MoveDown_Click(object sender, EventArgs e)
        {
            if (List_DataRow_Parts.SelectedIndex != MyronWebUAT.admin_notSelected &&
                List_DataRow_Parts.SelectedIndex != List_DataRow_Parts.Items.Count - 1 && currentDataRow != null)
            {
                int newPosition = List_DataRow_Parts.SelectedIndex + 1;
                string temp = currentDataRow.dataRowParts[List_DataRow_Parts.SelectedIndex];
                currentDataRow.dataRowParts[List_DataRow_Parts.SelectedIndex] = currentDataRow.dataRowParts[newPosition];
                currentDataRow.dataRowParts[newPosition] = temp;
                FillDataRowPartsList(currentDataRow.dataRowParts);
                List_DataRow_Parts.SelectedIndex = newPosition;
            }
        }

        private void Btn_Temp_Add_Click(object sender, EventArgs e)
        {
            //If the cmb has a datarow selected
            if (Cmb_Temp_DataRows.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                //Check the section where it will be added
                if (Radio_Temp_ItemSize.Checked)
                {
                    currentTemplate.itemSizeDataRowList.Add(controller.dataRows[Cmb_Temp_DataRows.SelectedIndex].dataRowID);
                    RefreshTemplateItemSizeList();
                    Cmb_Temp_DataRows.SelectedIndex = MyronWebUAT.admin_notSelected;
                }
                else if (Radio_Temp_ImprintArea.Checked)
                {
                    currentTemplate.imprintAreaDataRowList.Add(controller.dataRows[Cmb_Temp_DataRows.SelectedIndex].dataRowID);
                    RefreshTemplateImprintAreaList();
                    Cmb_Temp_DataRows.SelectedIndex = MyronWebUAT.admin_notSelected;
                }
                else if (Radio_Temp_Charges.Checked)
                {
                    currentTemplate.chargesDataRowList.Add(controller.dataRows[Cmb_Temp_DataRows.SelectedIndex].dataRowID);
                    RefreshTemplateChargesList();
                    Cmb_Temp_DataRows.SelectedIndex = MyronWebUAT.admin_notSelected;
                }
            }
        }

        /// <summary>
        /// Refreshes the Item Size List based on the content of currentTemplate.itemSizeDataRowList
        /// </summary>
        private void RefreshTemplateItemSizeList()
        {
            List_Temp_ItemSize.Items.Clear();
            for (int i = 0; i < currentTemplate.itemSizeDataRowList.Count; i++)
            {
                List_Temp_ItemSize.Items.Add(controller.FindDataRowById(currentTemplate.itemSizeDataRowList[i]).description);
            }
        }

        /// <summary>
        /// Refreshes the Imprint Area List based on the content of currentTemplate.imprintAreaDataRowList
        /// </summary>
        private void RefreshTemplateImprintAreaList()
        {
            List_Temp_ImprintArea.Items.Clear();
            for (int i = 0; i < currentTemplate.imprintAreaDataRowList.Count; i++)
            {
                List_Temp_ImprintArea.Items.Add(controller.FindDataRowById(currentTemplate.imprintAreaDataRowList[i]).description);
            }
        }

        /// <summary>
        /// Refreshes the Charges List based on the content of currentTemplate.imprintAreaDataRowList
        /// </summary>
        private void RefreshTemplateChargesList()
        {
            List_Temp_Charges.Items.Clear();
            for (int i = 0; i < currentTemplate.chargesDataRowList.Count; i++)
            {
                List_Temp_Charges.Items.Add(controller.FindDataRowById(currentTemplate.chargesDataRowList[i]).description);
            }
        }

        private void List_Temp_ItemSize_MouseClick(object sender, MouseEventArgs e)
        {
            int selected = List_Temp_ItemSize.SelectedIndex;
            List_Temp_ImprintArea.SelectedIndex = MyronWebUAT.admin_notSelected;
            List_Temp_Charges.SelectedIndex = MyronWebUAT.admin_notSelected;
            List_Temp_ItemSize.SelectedIndex = selected;
        }

        private void List_Temp_ImprintArea_MouseClick(object sender, MouseEventArgs e)
        {
            int selected = List_Temp_ImprintArea.SelectedIndex;
            List_Temp_ItemSize.SelectedIndex = MyronWebUAT.admin_notSelected;
            List_Temp_Charges.SelectedIndex = MyronWebUAT.admin_notSelected;
            List_Temp_ImprintArea.SelectedIndex = selected;
        }

        private void List_Temp_Charges_MouseClick(object sender, MouseEventArgs e)
        {
            int selected = List_Temp_Charges.SelectedIndex;
            List_Temp_ItemSize.SelectedIndex = MyronWebUAT.admin_notSelected;
            List_Temp_ImprintArea.SelectedIndex = MyronWebUAT.admin_notSelected;
            List_Temp_Charges.SelectedIndex = selected;
        }

        private void Btn_Temp_DeleteDataRow_Click(object sender, EventArgs e)
        {
            if (List_Temp_ItemSize.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                currentTemplate.itemSizeDataRowList.RemoveAt(List_Temp_ItemSize.SelectedIndex);
                RefreshTemplateItemSizeList();
            }
            else if (List_Temp_ImprintArea.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                currentTemplate.imprintAreaDataRowList.RemoveAt(List_Temp_ImprintArea.SelectedIndex);
                RefreshTemplateImprintAreaList();
            }
            else if (List_Temp_Charges.SelectedIndex != MyronWebUAT.admin_notSelected)
            {
                currentTemplate.chargesDataRowList.RemoveAt(List_Temp_Charges.SelectedIndex);
                RefreshTemplateChargesList();
            }
        }

        private void Btn_Temp_MoveUp_Click(object sender, EventArgs e)
        {
            if (List_Temp_ItemSize.SelectedIndex > 0 && currentTemplate != null)
            {
                int newPosition = List_Temp_ItemSize.SelectedIndex - 1;
                string temp = currentTemplate.itemSizeDataRowList[newPosition];
                currentTemplate.itemSizeDataRowList[newPosition] = currentTemplate.itemSizeDataRowList[List_Temp_ItemSize.SelectedIndex];
                currentTemplate.itemSizeDataRowList[List_Temp_ItemSize.SelectedIndex] = temp;
                RefreshTemplateItemSizeList();
                List_Temp_ItemSize.SelectedIndex = newPosition;
            }
            else if (List_Temp_ImprintArea.SelectedIndex > 0 && currentTemplate != null)
            {
                int newPosition = List_Temp_ImprintArea.SelectedIndex - 1;
                string temp = currentTemplate.imprintAreaDataRowList[newPosition];
                currentTemplate.imprintAreaDataRowList[newPosition] = currentTemplate.imprintAreaDataRowList[List_Temp_ImprintArea.SelectedIndex];
                currentTemplate.imprintAreaDataRowList[List_Temp_ImprintArea.SelectedIndex] = temp;
                RefreshTemplateImprintAreaList ();
                List_Temp_ImprintArea.SelectedIndex = newPosition;
            }
            else if (List_Temp_Charges.SelectedIndex > 0 && currentTemplate != null)
            {
                int newPosition = List_Temp_Charges.SelectedIndex - 1;
                string temp = currentTemplate.chargesDataRowList[newPosition];
                currentTemplate.chargesDataRowList[newPosition] = currentTemplate.chargesDataRowList[List_Temp_Charges.SelectedIndex];
                currentTemplate.chargesDataRowList[List_Temp_Charges.SelectedIndex] = temp;
                RefreshTemplateChargesList();
                List_Temp_Charges.SelectedIndex = newPosition;
            }
        }

        private void Btn_Temp_MoveDown_Click(object sender, EventArgs e)
        {
            if (List_Temp_ItemSize.SelectedIndex != MyronWebUAT.admin_notSelected &&
                List_Temp_ItemSize.SelectedIndex != List_Temp_ItemSize.Items.Count - 1 && currentTemplate != null)
            {
                int newPosition = List_Temp_ItemSize.SelectedIndex + 1;
                string temp = currentTemplate.itemSizeDataRowList[newPosition];
                currentTemplate.itemSizeDataRowList[newPosition] = currentTemplate.itemSizeDataRowList[List_Temp_ItemSize.SelectedIndex];
                currentTemplate.itemSizeDataRowList[List_Temp_ItemSize.SelectedIndex] = temp;
                RefreshTemplateItemSizeList();
                List_Temp_ItemSize.SelectedIndex = newPosition;
            }
            else if (List_Temp_ImprintArea.SelectedIndex != MyronWebUAT.admin_notSelected &&
                    List_Temp_ImprintArea.SelectedIndex != List_Temp_ImprintArea.Items.Count - 1 && currentTemplate != null)
            {
                int newPosition = List_Temp_ImprintArea.SelectedIndex + 1;
                string temp = currentTemplate.imprintAreaDataRowList[newPosition];
                currentTemplate.imprintAreaDataRowList[newPosition] = currentTemplate.imprintAreaDataRowList[List_Temp_ImprintArea.SelectedIndex];
                currentTemplate.imprintAreaDataRowList[List_Temp_ImprintArea.SelectedIndex] = temp;
                RefreshTemplateImprintAreaList();
                List_Temp_ImprintArea.SelectedIndex = newPosition;
            }
            else if (List_Temp_Charges.SelectedIndex != MyronWebUAT.admin_notSelected &&
                    List_Temp_Charges.SelectedIndex != List_Temp_Charges.Items.Count - 1 && currentTemplate != null)
            {
                int newPosition = List_Temp_Charges.SelectedIndex + 1;
                string temp = currentTemplate.chargesDataRowList[newPosition];
                currentTemplate.chargesDataRowList[newPosition] = currentTemplate.chargesDataRowList[List_Temp_Charges.SelectedIndex];
                currentTemplate.chargesDataRowList[List_Temp_Charges.SelectedIndex] = temp;
                RefreshTemplateChargesList();
                List_Temp_Charges.SelectedIndex = newPosition;
            }
        }
    }
}
