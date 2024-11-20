window.initializeFlowbite = function () {
    initFlowbite();
};
document.addEventListener('DOMContentLoaded', (event) => {
    window.initializeFlowbite();
});

// On page load or when changing themes, best to add inline in `head` to avoid FOUC
if (localStorage.getItem('color-theme') === 'dark' || (!('color-theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
    document.documentElement.classList.add('dark');
} else {
    document.documentElement.classList.remove('dark')
}

var themeToggleDarkIcon = document.getElementById('theme-toggle-dark-icon');
var themeToggleLightIcon = document.getElementById('theme-toggle-light-icon');

// Change the icons inside the button based on previous settings
if (localStorage.getItem('color-theme') === 'dark' || (!('color-theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
    themeToggleLightIcon.classList.remove('hidden');
} else {
    themeToggleDarkIcon.classList.remove('hidden');
}

var themeToggleBtn = document.getElementById('theme-toggle');

themeToggleBtn.addEventListener('click', function() {

    // toggle icons inside button
    themeToggleDarkIcon.classList.toggle('hidden');
    themeToggleLightIcon.classList.toggle('hidden');

    // if set via local storage previously
    if (localStorage.getItem('color-theme')) {
        if (localStorage.getItem('color-theme') === 'light') {
            document.documentElement.classList.add('dark');
            localStorage.setItem('color-theme', 'dark');
        } else {
            document.documentElement.classList.remove('dark');
            localStorage.setItem('color-theme', 'light');
        }

        // if NOT set via local storage previously
    } else {
        if (document.documentElement.classList.contains('dark')) {
            document.documentElement.classList.remove('dark');
            localStorage.setItem('color-theme', 'light');
        } else {
            document.documentElement.classList.add('dark');
            localStorage.setItem('color-theme', 'dark');
        }
    }

});

// Function to attach the remove event to items already in the DOM
function attachRemoveEvent(item) {
    const removeButton = item.querySelector('.remove-item-button');
    if (removeButton) {
        removeButton.addEventListener('click', function () {
            const container = item.closest('ul');
            item.remove(); // Remove the item
            updateIndices(container, ['input', 'select', 'textarea']); // Update indices for remaining items
        });
    } else {
        console.error('Remove button not found in the item!');
    }
}

// Function to handle adding a new item
document.querySelectorAll('.add-item-button').forEach(button => {
    button.addEventListener('click', function () {
        const containerId = this.getAttribute('data-target-container');
        const templateId = this.getAttribute('data-template-id');

        const container = document.getElementById(containerId);
        const template = document.getElementById(templateId);

        if (container && template) {
            const newItemFragment = template.content.cloneNode(true); // Clone the template content
            const tempDiv = document.createElement('div'); // Temporary wrapper to extract elements
            tempDiv.appendChild(newItemFragment);

            const newIndex = container.children.length;

            // Update the name and id attributes for all inputs
            tempDiv.querySelectorAll('[name]').forEach(input => {
                const name = input.getAttribute('name');
                if (name) {
                    input.setAttribute('name', name.replace('INDEX', newIndex));
                }
            });

            tempDiv.querySelectorAll('[id]').forEach(input => {
                const id = input.getAttribute('id');
                if (id) {
                    input.setAttribute('id', id.replace('INDEX', newIndex));
                }
            });

            const newItem = tempDiv.firstElementChild; // Extract the first item as the actual new DOM element
            container.appendChild(newItem); // Add the new item to the container

            attachRemoveEvent(newItem); // Attach the remove event to the new item
        } else {
            console.error('Container or template not found!');
        }
    });
});

// Function to update indices of remaining items
function updateIndices(container, inputSelectors) {
    const items = container.querySelectorAll('.item-entry');
    items.forEach((item, index) => {
        inputSelectors.forEach(selector => {
            item.querySelectorAll(selector).forEach(input => {
                const name = input.getAttribute('name');
                if (name) {
                    input.setAttribute('name', name.replace(/\[\d+\]/, `[${index}]`));
                }
                const id = input.getAttribute('id');
                if (id) {
                    input.setAttribute('id', id.replace(/\d+/, index));
                }
            });
        });
    });
}

// Attach remove event to existing items on page load
document.querySelectorAll('.item-entry').forEach(item => {
    attachRemoveEvent(item);
});
