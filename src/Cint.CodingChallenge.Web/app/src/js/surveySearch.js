const initialize = async () => {
    console.log('initialize...');
    const htmx = await import('htmx.org');
    const bootstrap = await import('bootstrap');
    const modalId = "modals-here";
    const modal = new bootstrap.Modal(document.getElementById(modalId));

    htmx.on("htmx:beforeSwap", (e) => {
        // Empty response targeting #dialog => hide the modal
        if (e.detail.target.id == modalId && e.detail.xhr.response == '') {
            modal.hide()
        }
    })
};

export default {
    initialize
};
