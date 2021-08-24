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

function validatePreferredMethodOfCommunication(executionContext) {
    var formContext = executionContext.getFormContext();
    
    let PreferredMethodOfCommunication = formContext.getControl('preferredcontactmethodcode').getAttribute().getValue();

    switch (PreferredMethodOfCommunication) {
        case 1: // Preferred Method = Any
            clearAllMandatoryFields(formContext);
            break;
        case 2: // Preferred Method = Email
            {
                clearAllMandatoryFields(formContext);
                formContext.getAttribute('emailaddress1').setRequiredLevel('required');
                break;
            }
        case 3: //Preferred Method = Phone
            {
                clearAllMandatoryFields(formContext);
                formContext.getAttribute('mobilephone').setRequiredLevel('required');
                break;
            }
        default:
            clearAllMandatoryFields(formContext);
            break;
    }
}

function clearAllMandatoryFields(formContext) {

    formContext.getAttribute('emailaddress1').setRequiredLevel('none');
    formContext.getAttribute('mobilephone').setRequiredLevel('none');
}

function ExtentInvestment(numberOfMonths, primaryControl) {
    //var contact = Xrm.Page.data.entity;
    console.log(primaryControl);
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
        Xrm.Page.getAttribute('statusCode').setValue('Matured');

        Xrm.Page.data.entity.save();

        Xrm.Page.data.refresh();
    }
    else {
        Xrm.Page.ui.setFormNotification("Investment havn't matured yet. Maturity date is" + Xrm.Page.data.entity.InvestmentPeriodMonths.toString());
    }
}