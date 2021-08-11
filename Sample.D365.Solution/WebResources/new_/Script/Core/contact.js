function disableNameInput() {
    Xrm.Page.getControl('Individual Client').setDisabled(true);
    Xrm.Page.getControl('First Name').setDisabled(true);
    Xrm.Page.getControl('Last Name').setDisabled(true);
    Xrm.Page.getControl('Corporate Client Name').setDisabled(true);
}

function validateTypeOfClient() {
    disableNameInput();

    switch (prefferedContactMethodCode) {
        case 339520000: // Type = Corporate Client
            {
                hideIndividualClientDetails();
                enableCorporateClientNameInput();
                break;
            }
        case 339520001: // Type = Individual Client
            {
                hideCorporateClientDetails();
                enableIndividualClientInput();
                break;
            }
    }
}

function enableCorporateClientNameInput() {
    Xrm.Page.getControl('Corporate Client Name').setDisabled(false);
}
function enableIndividualClientInput() {
    Xrm.Page.getControl('Individual Client').setDisabled(false);
    Xrm.Page.getControl('First Name').setDisabled(false);
    Xrm.Page.getControl('Last Name').setDisabled(false);
}

function hideCorporateClientDetails() {
    Xrm.Page.getControl('Corporate Client Name').setVisible(false);
}

function hideIndividualClientDetails() {
    Xrm.Page.getControl('Individual Client').setVisible(false);
    Xrm.Page.getControl('First Name').setVisible(false);
    Xrm.Page.getControl('Last Name').setVisible(false);
}

function validatePreferredMethodOfCommunication() {
    switch (prefferedContactMethodCode) {
        case 1: // Preferred Method = Any
            clearAllMandatoryFields();
            break;
        case 2: // Preferred Method = Email
            {
                clearAllMandatoryFields();
                Xrm.Page.getAttribute('Email').setRequiredLevel('required');
                break;
            }
        case 3: //Preferred Method = Phone
            {
                clearAllMandatoryFields();
                Xrm.Page.getAttribute('Mobile Phone').setRequiredLevel('required');
                break;
            }
    }
}

function clearAllMandatoryFields() {

    Xrm.Page.getAttribute('Email').setRequiredLevel('none');
    Xrm.Page.getAttribute('Mobile Phone').setRequiredLevel('none');
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
        Xrm.Page.getAttribute('Status Reason').setValue('Matured');

        Xrm.Page.data.entity.save();

        Xrm.Page.data.refresh();
    }
    else {
        Xrm.Page.ui.setFormNotification("Investment havn't matured yet. Maturity date is" + Xrm.Page.data.entity.InvestmentPeriodMonths.toString(), ...);
    }
}