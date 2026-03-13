// Initialize Notiflix with the main blue theme
const primaryBlue = '#2563eb';

const getNotiflixTheme = () => {
    const isDark = document.documentElement.classList.contains('dark');
    return {
        backgroundColor: isDark ? '#1f2937' : '#ffffff',
        titleColor: isDark ? '#f3f4f6' : '#111827',
        messageColor: isDark ? '#d1d5db' : '#374151',
        buttonColor: '#ffffff',
    };
}

const applyNotiflixTheme = () => {
    const theme = getNotiflixTheme();
    
    Notiflix.Report.init({
        width: '360px',
        borderRadius: '12px',
        titleFontSize: '18px',
        messageFontSize: '15px',
        buttonFontSize: '14px',
        svgSize: '45px',
        backgroundColor: theme.backgroundColor,
        success: {
            svgColor: primaryBlue,
            titleColor: theme.titleColor,
            messageColor: theme.messageColor,
            buttonBackground: primaryBlue,
            buttonColor: theme.buttonColor,
            backOverlayColor: 'rgba(59, 130, 246, 0.1)',
        },
        failure: {
            svgColor: '#ef4444',
            titleColor: theme.titleColor,
            messageColor: theme.messageColor,
            buttonBackground: '#ef4444',
            buttonColor: theme.buttonColor,
            backOverlayColor: 'rgba(239, 68, 68, 0.1)',
        }
    });

    Notiflix.Confirm.init({
        width: '320px',
        borderRadius: '12px',
        backgroundColor: theme.backgroundColor,
        titleColor: primaryBlue,
        messageColor: theme.messageColor,
        okButtonBackground: primaryBlue,
        cancelButtonBackground: theme.backgroundColor === '#1f2937' ? '#374151' : '#f3f4f6',
        cancelButtonColor: theme.backgroundColor === '#1f2937' ? '#9ca3af' : '#4b5563',
        titleFontSize: '18px',
        messageFontSize: '15px',
        buttonsFontSize: '14px',
    });
}

// Initial apply
applyNotiflixTheme();

// Re-apply when theme changes
const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
        if (mutation.attributeName === 'class') {
            applyNotiflixTheme();
        }
    });
});
observer.observe(document.documentElement, { attributes: true });

window.successMessage = (message) => {
    Notiflix.Report.success('Success', message, 'Done');
}

window.errorMessage = (message) => {
    Notiflix.Report.failure('Error', message, 'Ok');
}

window.ConfirmMessageBox = (message) => {
    return new Promise((resolve) => {
        Notiflix.Confirm.show(
            'Confirmation',
            message,
            'Yes, Continue',
            'No, Cancel',
            function okCb() { resolve(true); },
            function cancelCb() { resolve(false); }
        );
    });
};
