function disableNameInput(formContext) {

    //formContext.getControl('fullname').setDisabled(true);
    formContext.getControl('fullname_compositionLinkControl_firstname').setDisabled(true);
    formContext.getControl('fullname_compositionLinkControl_middlename').setDisabled(true);
    formContext.getControl('fullname_compositionLinkControl_lastname').setDisabled(true);
    formContext.getControl('cr53a_corporateclientname').setDisabled(true);
}

function validateTypeOfClient(executionContext) {
    var formContext = executionContext.getFormContext();

    disableNameInput(formContext);

    let prefferedContactMethodCode = formContext.getControl('cr53a_typeofclient').getAttribute().getValue();
    switch (prefferedContactMethodCode) {
        case 339520000: // Type = Corporate Client
            {
                hideIndividualClientDetails(formContext);
                enableCorporateClientNameInput(formContext);
                break;
            }
        case 339520001: // Type = Individual Client
            {
                hideCorporateClientDetails(formContext);
                enableIndividualClientInput(formContext);
                break;
            }
    }
}

function enableCorporateClientNameInput(formContext) {
    formContext.getControl('cr53a_corporateclientname').setDisabled(false);
    formContext.getControl('cr53a_corporateclientname').setVisible(true);
}

function enableIndividualClientInput(formContext) {
    formContext.getControl('fullname_compositionLinkControl_firstname').setDisabled(false);
    formContext.getControl('fullname_compositionLinkControl_middlename').setDisabled(false);
    formContext.getControl('fullname_compositionLinkControl_lastname').setDisabled(false);

    formContext.getControl('fullname_compositionLinkControl_firstname').setVisible(true);
    formContext.getControl('fullname_compositionLinkControl_middlename').setVisible(true);
    formContext.getControl('fullname_compositionLinkControl_lastname').setVisible(true);
}

function hideCorporateClientDetails(formContext) {
    formContext.getControl('cr53a_corporateclientname').setVisible(false);
}

function hideIndividualClientDetails(formContext) {
    formContext.getControl('fullname_compositionLinkControl_firstname').setVisible(false);
    formContext.getControl('fullname_compositionLinkControl_middlename').setVisible(false);
    formContext.getControl('fullname_compositionLinkControl_lastname').setVisible(false);
}

function validatePreferredMethodOfCommunication() {

    let PreferredMethodOfCommunication = Xrm.Page.getControl('PreferredContactMethodCode').getValue();

    switch (PreferredMethodOfCommunication) {
        case 1: // Preferred Method = Any
            clearAllMandatoryFields();
            break;
        case 2: // Preferred Method = Email
            {
                clearAllMandatoryFields();
                Xrm.Page.getAttribute('email').setRequiredLevel('required');
                break;
            }
        case 3: //Preferred Method = Phone
            {
                clearAllMandatoryFields();
                Xrm.Page.getAttribute('mobilephone').setRequiredLevel('required');
                break;
            }
    }
}

function clearAllMandatoryFields() {

    Xrm.Page.getAttribute('email').setRequiredLevel('none');
    Xrm.Page.getAttribute('mobilephone').setRequiredLevel('none');
}

function ExtentInvestment(numberOfMonths) {
    //var contact = Xrm.Page.data.entity;

    Xrm.Page.data.entity.InvestmentPeriodMonths += numberOfMonths;

    Xrm.Page.data.entity.save();

    Xrm.Page.data.refresh();
}

function SetMatured() {
    var date = new Date();

    if (Xrm.Page.data.entity.InvestmentPeriodMonths <= date) {

        var lookupVal = new Array();

        lookupVal[0] = new Object();

        lookupVal[0].id = Xrm.Page.data.entity.contactid;

        lookupVal[0].name = Xrm.Page.data.entity.firstname;

        lookupVal[0].entityType = "contact";
        Xrm.Page.getAttribute('StatusCode').setValue('Matured');

        Xrm.Page.data.entity.save();

        Xrm.Page.data.refresh();
    }
    else {
        Xrm.Page.ui.setFormNotification("Investment havn't matured yet. Maturity date is" + Xrm.Page.data.entity.InvestmentPeriodMonths.toString());
    }
}