document.addEventListener('DOMContentLoaded', function() {
    function toggleDropdown(event) {
        event.stopPropagation();
        var dropdownMenu = document.getElementById('dropdownMenu');
        dropdownMenu.classList.toggle('hidden');
    }

    document.addEventListener('click', function(event) {
        var dropdownMenu = document.getElementById('dropdownMenu');
        if (!dropdownMenu.contains(event.target) && event.target.id !== 'userDropdown') {
            dropdownMenu.classList.add('hidden');
        }
    });

    document.getElementById('userDropdown').addEventListener('click', function(event) {
        toggleDropdown(event);
    });
});
