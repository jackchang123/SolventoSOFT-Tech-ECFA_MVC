document.addEventListener("DOMContentLoaded", function () {
    // 1. 🌟 修正選取器：直接選取 form 裡面的 submit 按鈕
    const searchButtons = document.querySelectorAll('.search-bar button[type="submit"]');

    searchButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            // 阻止表單預設的送出重新整理行為
            event.preventDefault();

            // 2. 🌟 修正容器名稱：尋找當前按鈕所屬的 .search-bar
            const container = this.closest('.search-bar');
            if (!container) return;

            // 3. 從當前容器中，只找出對應的輸入框與錯誤提示
            const inputField = container.querySelector('.search-input');
            const errorPrompt = container.querySelector('.error-prompt');

            // 4. 基本驗證：防範空值或只有空白鍵
            const rawKeyword = inputField.value.trim();
            if (rawKeyword === '') {
                if (errorPrompt) errorPrompt.style.display = 'block'; // 顯示輸入錯誤
                inputField.focus();
                return;
            } else {
                if (errorPrompt) errorPrompt.style.display = 'none';  // 隱藏錯誤提示
            }

            // 5. 🛡️ 資安防護 (XSS 防禦)：使用 encodeURIComponent 將特殊字元編碼
            const safeKeyword = encodeURIComponent(rawKeyword);

            // 6. 導向目標網頁
            window.location.href = `/search?keyword=${safeKeyword}`;
        });
    });
});