const primaryBlue = '#2563eb';

// Helper to convert hex to HSL components (just the numbers)
const hexToHslComponents = (hex) => {
    let r = parseInt(hex.slice(1, 3), 16) / 255;
    let g = parseInt(hex.slice(3, 5), 16) / 255;
    let b = parseInt(hex.slice(5, 7), 16) / 255;
    let max = Math.max(r, g, b), min = Math.min(r, g, b);
    let h, s, l = (max + min) / 2;
    if (max === min) { h = s = 0; }
    else {
        let d = max - min;
        s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
        switch (max) {
            case r: h = (g - b) / d + (g < b ? 6 : 0); break;
            case g: h = (b - r) / d + 2; break;
            case b: h = (r - g) / d + 4; break;
        }
        h /= 6;
    }
    return `${(h * 360).toFixed(1)} ${(s * 100).toFixed(1)}% ${(l * 100).toFixed(1)}%`;
}

// Helper to get brightness (0-1)
const getBrightness = (hex) => {
    let r = parseInt(hex.slice(1, 3), 16);
    let g = parseInt(hex.slice(3, 5), 16);
    let b = parseInt(hex.slice(5, 7), 16);
    return (r * 299 + g * 587 + b * 114) / 1000 / 255;
}

window.themeManager = {
    getTheme: () => localStorage.getItem('theme') || (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'),
    setTheme: (theme) => {
        localStorage.setItem('theme', theme);
        if (theme === 'dark') {
            document.documentElement.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
        }
    },
    getPrimaryColor: () => localStorage.getItem('primaryColor'),
    setPrimaryColor: (hex) => {
        if (!hex) {
            localStorage.removeItem('primaryColor');
            document.documentElement.style.removeProperty('--primary');
            document.documentElement.style.removeProperty('--primary-foreground');
            window.themeManager.updateFavicon('#3B82F6');
            return;
        }
        localStorage.setItem('primaryColor', hex);
        const hsl = hexToHslComponents(hex);
        document.documentElement.style.setProperty('--primary', hsl);
        document.documentElement.style.setProperty('--primary-foreground', getBrightness(hex) > 0.7 ? '0 0% 0%' : '0 0% 100%');
        window.themeManager.updateFavicon(hex);
    },
    updateFavicon: (hex) => {
        const favicon = document.getElementById('favicon');
        if (!favicon) return;
        
        const svgContent = `
            <svg width="32" height="32" viewBox="0 0 32 32" fill="none" xmlns="http://www.w3.org/2000/svg">
              <rect width="32" height="32" rx="6" fill="${hex}"/>
              <path d="M16 8L8 12V20L16 24L24 20V12L16 8Z" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M16 12L24 16L16 20L8 16L16 12Z" stroke="white" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
              <path d="M16 24V20" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
        `.trim();
        
        const blob = new Blob([svgContent], { type: 'image/svg+xml' });
        const url = URL.createObjectURL(blob);
        favicon.href = url;
    }
};

// Apply saved settings on load
(function() {
    const theme = themeManager.getTheme();
    themeManager.setTheme(theme);
    const primary = themeManager.getPrimaryColor();
    if (primary) themeManager.setPrimaryColor(primary);
})();

const getNotiflixTheme = () => {
    const isDark = document.documentElement.classList.contains('dark');
    const primary = themeManager.getPrimaryColor() || primaryBlue;
    return {
        backgroundColor: isDark ? '#1f2937' : '#ffffff',
        titleColor: isDark ? '#f3f4f6' : '#111827',
        messageColor: isDark ? '#d1d5db' : '#374151',
        buttonColor: '#ffffff',
        primary: primary
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
            svgColor: theme.primary,
            titleColor: theme.titleColor,
            messageColor: theme.messageColor,
            buttonBackground: theme.primary,
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
        titleColor: theme.primary,
        messageColor: theme.messageColor,
        okButtonBackground: theme.primary,
        cancelButtonBackground: theme.backgroundColor === '#1f2937' ? '#374151' : '#f3f4f6',
        cancelButtonColor: theme.backgroundColor === '#1f2937' ? '#9ca3af' : '#4b5563',
        titleFontSize: '18px',
        messageFontSize: '15px',
        buttonsFontSize: '14px',
    });
}

// Initial apply
applyNotiflixTheme();

// Re-apply when theme changes (including custom color since it sets style on html)
const observer = new MutationObserver((mutations) => {
    mutations.forEach((mutation) => {
        if (mutation.attributeName === 'class' || mutation.attributeName === 'style') {
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
