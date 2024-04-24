function update(instrument) {
    const selector = column => `#${column}_${instrument.symbol}`
    const cell = column => $(selector(column))

    const columns = ['last', 'open', 'low', 'high']

    columns.forEach(column => {
        cell(column).text(instrument.quotes[column].toFixed(2))
    })

    cell('volume').text(instrument.quotes.volume)
    cell('lastTimestamp').text(new Date(instrument.quotes.lastTimestamp).toLocaleTimeString())

    const previous = cell('last').val() ?? 0.0
    cell('last').val(instrument.quotes.last)

    const row = $(`#${instrument.symbol}`)
    if (instrument.quotes.last > previous) {
        row.addClass('green')
        setTimeout(() => row.removeClass('green'), 700)
    }

    if (instrument.quotes.last < previous) {
        row.addClass('red')
        setTimeout(() => row.removeClass('red'), 700)
    }
}
function createConnection(token) {
    return new signalR.HubConnectionBuilder()
        .withUrl(`/stream`)
        .configureLogging(signalR.LogLevel.Information)
        .build();
}

async function startQuoteFeed(token, updateFn) {
    const connection = createConnection(token)
    try {
        await connection.start();
        console.log("SignalR Connected.");
        connection.on('update', update)
    } catch (err) {
        console.log(err);
        setTimeout(() => startQuoteFeed(token), 5000);
    }
}