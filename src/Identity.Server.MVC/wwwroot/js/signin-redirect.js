const dirtyUrl = document.querySelector("meta[http-equiv=refresh]").getAttribute("data-url");
const cleanUrl = DOMPurify.sanitize(dirtyUrl);
window.location.href = cleanUrl;
import DOMPurify from 'dompurify';
