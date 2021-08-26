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
    var value = parseInt(primaryControl.getControl('cr53a_investmentperiodmonths').getValue());
    var newInvestmentPeriod = value + numberOfMonths;
    primaryControl.getAttribute('cr53a_investmentperiodmonths').setValue(newInvestmentPeriod);

    primaryControl.data.entity.save();

    setTimeout(function () {
        // Call the Open Entity Form method and pass through the current entity name and ID to force CRM to reload the record

        Xrm.Utility.openEntityForm(primaryControl.data.entity.getEntityName(), primaryControl.data.entity.getId());
    }, 3000);
}

function SetMatured(primaryControl) {
    var date = new Date();

    var investmentPeriod = parseInt(primaryControl.getAttribute('cr53a_investmentperiodmonths').getValue());
    var joinDate = new Date(primaryControl.getAttribute('cr53a_joiningdate').getValue());

    var maturityDate = new Date(joinDate.setMonth(joinDate.getMonth() + investmentPeriod));

    if (maturityDate <= date) {

        primaryControl.getAttribute('statuscode').setValue(2);
        primaryControl.getAttribute('statecode').setValue(1);

        primaryControl.data.entity.save();

        setTimeout(function () {
            // Call the Open Entity Form method and pass through the current entity name and ID to force CRM to reload the record

            Xrm.Utility.openEntityForm(primaryControl.data.entity.getEntityName(), primaryControl.data.entity.getId());
        }, 3000);
    }
    else {
        primaryControl.ui.setFormNotification("Investment havn't matured yet. Maturity date is " + maturityDate.toDateString(), "INFO", Date.now());
    }
}