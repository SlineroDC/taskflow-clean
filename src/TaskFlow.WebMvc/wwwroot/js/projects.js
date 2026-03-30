// --- MODALES DE PROYECTOS ---
function toggleProjectModal() {
    const modal = document.getElementById('newProjectModal');
    if(modal) modal.classList.toggle('hidden');
}

function openEditModal(id, name, desc) {
    document.getElementById('editProjectId').value = id;
    document.getElementById('editProjectName').value = name;
    document.getElementById('editProjectDesc').value = desc;
    document.getElementById('editForm').action = '/Projects/Edit/' + id;
    document.getElementById('editProjectModal').classList.remove('hidden');
}

function closeEditModal() {
    const modal = document.getElementById('editProjectModal');
    if(modal) modal.classList.add('hidden');
}

// --- MODALES DE TAREAS ---
function openTaskFormModal(mode, id = '', title = '', priorityStr = '') {
    const form = document.getElementById('taskForm');
    const titleEl = document.getElementById('taskFormTitle');
    const iconEl = document.getElementById('taskFormIcon');
    const submitBtn = document.getElementById('taskFormSubmitBtn');
    const headerBar = document.getElementById('taskFormHeaderBar');

    if (mode === 'add') {
        titleEl.innerText = 'Añadir Nueva Tarea';
        iconEl.innerText = 'add_task';
        iconEl.className = 'material-symbols-outlined text-primary';
        submitBtn.innerText = 'Añadir al Sprint';
        submitBtn.className = 'flex-1 py-3 px-4 rounded-lg bg-primary text-slate-900 font-bold text-sm hover:shadow-[0_0_20px_rgba(152,202,63,0.4)] transition-all';
        headerBar.className = 'absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-primary to-primary-container';
        form.action = '/Tasks/Create';
        
        document.getElementById('taskFormId').value = '';
        document.getElementById('taskFormTitleInput').value = '';
        document.getElementById('taskFormPriority').value = 1;
    } else {
        titleEl.innerText = 'Editar Tarea';
        iconEl.innerText = 'edit_note';
        iconEl.className = 'material-symbols-outlined text-tertiary-dim';
        submitBtn.innerText = 'Guardar Cambios';
        submitBtn.className = 'flex-1 py-3 px-4 rounded-lg bg-tertiary-dim text-slate-900 font-bold text-sm hover:shadow-[0_0_20px_rgba(240,224,74,0.4)] transition-all';
        headerBar.className = 'absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-tertiary-dim to-tertiary';
        form.action = '/Tasks/Edit';
        
        document.getElementById('taskFormId').value = id;
        document.getElementById('taskFormTitleInput').value = title;
        const priorityMap = { "Low": 0, "Medium": 1, "High": 2, "Urgent": 3 };
        document.getElementById('taskFormPriority').value = priorityMap[priorityStr] !== undefined ? priorityMap[priorityStr] : 1;
    }
    document.getElementById('taskFormModal').classList.remove('hidden');
}

function closeTaskFormModal() { document.getElementById('taskFormModal').classList.add('hidden'); }

function handleEditClick(btn) {
    openTaskFormModal('edit', btn.getAttribute('data-id'), btn.getAttribute('data-title'), btn.getAttribute('data-priority'));
}

// --- MODAL CONFIRMACIÓN ---
function openConfirmModal(title, icon, message, actionUrl, taskId = '', btnText = 'Confirmar', isDanger = false) {
    document.getElementById('confirmModalTitle').innerText = title;
    document.getElementById('confirmModalMessage').innerText = message;
    document.getElementById('confirmModalForm').action = actionUrl;
    document.getElementById('confirmModalTaskId').value = taskId;
    
    const submitBtn = document.getElementById('confirmModalSubmitBtn');
    const iconContainer = document.getElementById('confirmModalIconContainer');
    const iconEl = document.getElementById('confirmModalIcon');
    submitBtn.innerText = btnText;
    iconEl.innerText = icon;

    if (isDanger) {
        submitBtn.className = "flex-1 py-3 bg-error text-white rounded-lg font-bold text-sm hover:shadow-[0_0_20px_rgba(255,180,171,0.4)] transition-all";
        iconContainer.className = "w-16 h-16 bg-error/20 rounded-full flex items-center justify-center mx-auto mb-4 border border-error/30";
        iconEl.className = "material-symbols-outlined text-error text-3xl";
    } else {
        submitBtn.className = "flex-1 py-3 bg-primary text-slate-900 rounded-lg font-bold text-sm hover:shadow-[0_0_20px_rgba(152,202,63,0.4)] transition-all";
        iconContainer.className = "w-16 h-16 bg-primary/20 rounded-full flex items-center justify-center mx-auto mb-4 border border-primary/30";
        iconEl.className = "material-symbols-outlined text-primary text-3xl";
    }
    document.getElementById('confirmModal').classList.remove('hidden');
}

function closeConfirmModal() { document.getElementById('confirmModal').classList.add('hidden'); }

// --- INICIALIZADOR DRAG & DROP Y BÚSQUEDA ---
document.addEventListener('DOMContentLoaded', () => {
    // Buscador de Proyectos
    const searchInput = document.querySelector('input[placeholder="Buscar..."]');
    const projectCards = document.querySelectorAll('.grid > .bg-white, .grid > .bg-surface-container');
    if (searchInput && projectCards.length > 0) {
        searchInput.addEventListener('input', (e) => {
            const term = e.target.value.toLowerCase();
            projectCards.forEach(card => {
                const titleElement = card.querySelector('h3');
                if (titleElement) card.style.display = titleElement.textContent.toLowerCase().includes(term) ? 'flex' : 'none';
            });
        });
    }

    // Sortable (Tareas)
    const taskList = document.getElementById('taskList');
    if (taskList && typeof Sortable !== 'undefined') {
        new Sortable(taskList, {
            animation: 150,
            handle: '.cursor-grab',
            ghostClass: 'opacity-50',
            onEnd: function (evt) {
                const taskId = evt.item.getAttribute('data-task-id');
                const newOrder = evt.newIndex + 1;
                fetch(`/Tasks/Reorder?id=${taskId}&newOrder=${newOrder}`, { method: 'POST' });
            }
        });
    }
});