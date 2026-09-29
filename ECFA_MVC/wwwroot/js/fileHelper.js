// 確保全域工具箱存在
window.FileHelper = window.FileHelper || {};

// 定義 Mapping 對應表
window.FileHelper.iconMapping = {
    'pdf': 'icon_pdf_red.svg',
    'doc': 'icon_word.svg',
    'docx': 'icon_word.svg',
    'xls': 'icon_excel.svg',
    'xlsx': 'icon_excel.svg',
    'ppt': 'icon_related_links.svg',
    'pptx': 'icon_powerpoint.svg',
    'png': 'icon_image.svg',
    'jpg': 'icon_image.svg',
    'jpeg': 'icon_image.svg',
    'zip': 'icon_related_links.svg',
    'rar': 'icon_zip.svg'
};

window.FileHelper.defaultIcon = 'icon_related_links.svg';

// 定義共用方法
window.FileHelper.getFileIconSrc = function (fileName) {
    if (!fileName) return `../images/${window.FileHelper.defaultIcon}`;

    // 取出副檔名並轉小寫
    const ext = fileName.split('.').pop().toLowerCase();
    const iconName = window.FileHelper.iconMapping[ext] || window.FileHelper.defaultIcon;

    return `../images/${iconName}`;
};