//Notiflix.Report.init({
//    width: '250px', 
//    messageMaxLength: 200, 
//    titleFontSize: '18px', 
//    messageFontSize: '14px', 
//    buttonFontSize: '12px', 
//    backgroundColor: '#222222', 
//    titleColor: '#ffffff', 
//    messageColor: '#dddddd', 
//    buttonBackground: '#ff6b6b', 
//    buttonColor: '#ffffff', 
//});

window.successMessage = (message) => {
    console.log("Success");
    Notiflix.Report.success(
        'Success!',
        message,
        'Ok',
    );
}

window.errorMessage = (message) => {

    Notiflix.Report.failure(
        'Error!',
        message,
        'Ok',
    );
}

window.ConfirmMessageBox = (message) => {
    return new Promise((resolve) => {
        Notiflix.Confirm.show(
            'Confirm',
            message,
            'Yes',
            'No',
            function okCb() {
                resolve(true); // Resolve with true if "Yes" is clicked
            },
            function cancelCb() {
                resolve(false); // Resolve with false if "No" is clicked
            }
        );
    });
};
