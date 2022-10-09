// Отправка файла и получение его идентификатора
async function SendFileAsync(form, url) {

    let response = await fetch(url,
        {
            body: new FormData(form),
            method: "post"
        });

    if (response.status != 200) {
        alert(response.statusText);
        return -1;
    }

    return Number(await response.text());
}

function SendFile(form, url) {
    return SendFileAsync(form, url);
}

// Получение сообщений путём длинного опроса
function SubscribePane(elemId, url) {

    function showMessage(message) {
        let messageElem = document.getElementById(elemId);
        messageElem.innerHTML = message;
    }

    async function subscribe() {
        let response = await fetch(url);

        if (response.status == 502) {
            // Таймаут подключения
            // случается, когда соединение ждало слишком долго.
            // давайте восстановим связь
            await subscribe();
        } else if (response.status != 200) {
            // Показать ошибку
            showMessage(response.statusText);
            // Подключиться снова через секунду.
            await new Promise(resolve => setTimeout(resolve, 1000));
            await subscribe();
        } else {
            // Получить сообщение
            let message = await response.text();
            showMessage(message);

            // Если мы получили ссылку то не имеет смысла дальнейшие подключения
            if (message.indexOf("href") === -1) {
                await subscribe();
            }
        }
    }

    subscribe();
}