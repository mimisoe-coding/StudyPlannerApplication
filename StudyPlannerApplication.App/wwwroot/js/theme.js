window.themeManager = {
    setTheme: (theme) => {
        console.log('Setting theme to:', theme);
        if (theme === 'dark') {
            document.documentElement.classList.add('dark');
            if (document.body) document.body.classList.add('dark');
        } else {
            document.documentElement.classList.remove('dark');
            if (document.body) document.body.classList.remove('dark');
        }
        localStorage.setItem('theme', theme);
        return theme;
    },
    getTheme: () => {
        const stored = localStorage.getItem('theme');
        const theme = stored || (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
        console.log('Current theme:', theme);
        return theme;
    },
    init: () => {
        const theme = window.themeManager.getTheme();
        window.themeManager.setTheme(theme);
        
        // Ensure body gets the class once it's available
        if (!document.body) {
            document.addEventListener('DOMContentLoaded', () => {
                window.themeManager.setTheme(theme);
            });
        }
    }
};

// Initial call
window.themeManager.init();
