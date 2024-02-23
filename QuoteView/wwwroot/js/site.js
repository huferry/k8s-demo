// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function update(instrument) {
    const selector = column => `#${column}_${instrument.symbol}`
    const cell = column => $(selector(column))
    
    const columns = ['last', 'volume', 'open', 'low', 'high']
    
    columns.forEach(column => {
        cell(column).text(instrument.quotes[column])
    })
    
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

update({
    symbol: 'ABN',
    quotes: {
        last: 14.011,
        lastTimestamp: "2024-02-23T11:54:08.00Z",
        volume: 4221,
        open: 15.112,
        low: 13.889,
        high: 16.891
    }
})

update({
    symbol: 'ABN',
    quotes: {
        last: 14.211,
        lastTimestamp: "2024-02-23T11:54:10.00Z",
        volume: 4221,
        open: 15.112,
        low: 13.889,
        high: 16.891
    }
})