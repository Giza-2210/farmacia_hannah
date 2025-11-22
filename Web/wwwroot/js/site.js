// === Manejo de dropdowns principales (solo uno abierto a la vez) ===
document.querySelectorAll('.sidebar .dropdown-toggle').forEach(toggle => {
    toggle.addEventListener('click', function (e) {
        e.preventDefault();
        const parent = this.closest('.dropdown');

        // Evitar interferencia si el clic viene desde un submenu
        if (this.closest('.dropdown-submenu')) return;

        // Cerrar otros dropdowns principales
        document.querySelectorAll('.sidebar .dropdown').forEach(d => {
            if (d !== parent) d.classList.remove('show');
        });

        parent.classList.toggle('show');
    });
});


// === Activar submenús dentro de dropdowns ===
document.querySelectorAll('.dropdown-submenu > a').forEach(link => {
    link.addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        // Cerrar otros submenús hermanos
        const parentUl = this.closest('.dropdown-menu');
        parentUl.querySelectorAll('.dropdown-submenu .dropdown-menu.show').forEach(sub => {
            if (sub !== this.nextElementSibling) sub.classList.remove('show');
        });

        // Mostrar / ocultar el submenu actual
        const submenu = this.nextElementSibling;
        if (submenu) submenu.classList.toggle('show');
    });
});


// === Cerrar dropdowns al hacer clic fuera del sidebar ===
document.addEventListener('click', function (e) {
    if (!e.target.closest('.sidebar')) {
        document.querySelectorAll('.sidebar .dropdown').forEach(d => d.classList.remove('show'));
        document.querySelectorAll('.dropdown-submenu .dropdown-menu').forEach(sub => sub.classList.remove('show'));
    }
});


// === Mostrar / ocultar sidebar ===
const toggleBtn = document.getElementById('toggleSidebar');
const sidebar = document.getElementById('sidebar');

if (toggleBtn && sidebar) {
    toggleBtn.addEventListener('click', () => {
        sidebar.classList.toggle('collapsed');
    });
}


// === Inicializar todos los selectpicker ===
$('select').selectpicker();



