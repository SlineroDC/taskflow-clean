// --- ANTI-FLASHEO & TEMA ---
function toggleTheme() {
    const html = document.documentElement;
    const icon = document.getElementById('themeToggleIcon');
    
    if (html.classList.contains('dark')) {
        html.classList.remove('dark');
        localStorage.theme = 'light';
        if(icon) icon.textContent = 'light_mode';
    } else {
        html.classList.add('dark');
        localStorage.theme = 'dark';
        if(icon) icon.textContent = 'dark_mode';
    }
}

// --- SIDEBAR (Móvil) ---
function toggleSidebar() {
    const sidebar = document.getElementById('app-sidebar');
    const overlay = document.getElementById('mobileOverlay');
    
    if(sidebar && overlay) {
        sidebar.classList.toggle('-translate-x-full');
        if (sidebar.classList.contains('-translate-x-full')) {
            overlay.classList.remove('opacity-100');
            setTimeout(() => overlay.classList.add('hidden'), 300);
        } else {
            overlay.classList.remove('hidden');
            setTimeout(() => overlay.classList.add('opacity-100'), 10);
        }
    }
}

// --- AUTH: CONTRASEÑAS ---
function togglePasswordVisibility(inputId, iconId) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById(iconId);
    if(!input || !icon) return;
    
    if (input.type === 'password') {
        input.type = 'text';
        icon.textContent = 'visibility_off';
    } else {
        input.type = 'password';
        icon.textContent = 'visibility';
    }
}

function initPasswordMatchValidation(passwordId, confirmId) {
    const pwd = document.getElementById(passwordId);
    const confirmPwd = document.getElementById(confirmId);
    if(!pwd || !confirmPwd) return;

    const checkMatch = () => {
        if (confirmPwd.value === '') {
            confirmPwd.classList.remove('ring-2', 'ring-green-500', 'border-green-500', 'ring-error', 'border-error');
            return;
        }
        if (pwd.value === confirmPwd.value) {
            confirmPwd.classList.remove('focus:ring-primary', 'focus:ring-error', 'ring-error', 'border-error', 'dark:border-transparent');
            confirmPwd.classList.add('ring-2', 'ring-green-500', 'border-green-500', 'focus:ring-green-500');
        } else {
            confirmPwd.classList.remove('focus:ring-primary', 'ring-2', 'ring-green-500', 'border-green-500', 'focus:ring-green-500');
            confirmPwd.classList.add('ring-2', 'ring-error', 'border-error', 'focus:ring-error');
        }
    };
    pwd.addEventListener('input', checkMatch);
    confirmPwd.addEventListener('input', checkMatch);
}

// --- INICIALIZADOR GLOBAL ---
document.addEventListener('DOMContentLoaded', () => {
    // Configurar icono del tema
    const icon = document.getElementById('themeToggleIcon');
    if (icon) icon.textContent = document.documentElement.classList.contains('dark') ? 'dark_mode' : 'light_mode';

    // Auto-destruir Toasts
    setTimeout(() => {
        const successToast = document.getElementById('toast-success');
        const errorToast = document.getElementById('toast-error');
        if (successToast) successToast.remove();
        if (errorToast) errorToast.remove();
    }, 5000);

    // Inicializar validadores de contraseña si existen en la vista
    initPasswordMatchValidation('newPwd', 'confirmPwd');
    initPasswordMatchValidation('regPassword', 'regConfirm'); 
});