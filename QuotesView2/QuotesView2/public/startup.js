const initial = [
    {
        "symbol": "ABN",
        "name": "ABN Amro Bank N.V.",
        "quote": 15.045
    },
    {
        "symbol": "ADYEN",
        "name": "Adyen NV",
        "quote": 1499
    },
    {
        "symbol": "AGN",
        "name": "Aegon",
        "quote": 5.374
    },
    {
        "symbol": "ADYEN",
        "name": "Ahold Delhaize",
        "quote": 27.42
    },
    {
        "symbol": "AKZ",
        "name": "Akzo Nobel",
        "quote": 68.7
    },
    {
        "symbol": "MT",
        "name": "ArcelorMittal",
        "quote": 25.275
    },
    {
        "symbol": "ASM",
        "name": "ASMI",
        "quote": 575.1
    },
    {
        "symbol": "ASML",
        "name": "ASML",
        "quote": 863.5
    },
    {
        "symbol": "ASR",
        "name": "ASR Nederland",
        "quote": 43.22
    },
    {
        "symbol": "BSI",
        "name": "BESI",
        "quote": 160.65
    },
    {
        "symbol": "DSFIR",
        "name": "DSM Firmenich AG",
        "quote": 103.38
    },
    {
        "symbol": "EXO",
        "name": "Exor NV",
        "quote": 98.14
    },
    {
        "symbol": "HEI",
        "name": "Heineken",
        "quote": 89.04
    },
    {
        "symbol": "IMCD",
        "name": "IMCD",
        "quote": 141.65
    },
    {
        "symbol": "ING",
        "name": "ING",
        "quote": 12.374
    },
    {
        "symbol": "KPN",
        "name": "KPN",
        "quote": 3.34
    },
    {
        "symbol": "NN",
        "name": "NN Group",
        "quote": 37.32
    },
    {
        "symbol": "PHIA",
        "name": "Philips Koninklijke",
        "quote": 100.802
    },
    {
        "symbol": "PRX",
        "name": "Prosus",
        "quote": 28.59
    },
    {
        "symbol": "RAND",
        "name": "Randstad NV",
        "quote": 51.86
    },
    {
        "symbol": "REN",
        "name": "Relx",
        "quote": 39.74
    },
    {
        "symbol": "SHELL",
        "name": "Shell PLC",
        "quote": 29.62
    },
    {
        "symbol": "UMG",
        "name": "UMG",
        "quote": 26.6
    },
    {
        "symbol": "UNA",
        "name": "Unilever PLC",
        "quote": 47.145
    },
    {
        "symbol": "WKL",
        "name": "Wolters Kluwer",
        "quote": 144.7
    }
];

$('body').ready(() => {

    const table = $('#quote-table')
    initial.forEach(quote => {
        const row = $('<tr>').attr('id', quote.symbol).addClass('instrument')

        $('<td>').addClass('name').text(quote.name).appendTo(row)
        $('<td>').addClass('last price').attr('id', `last_${quote.symbol}`).appendTo(row)
        $('<td>').addClass('lastTimestamp price').attr('id', `lastTimestamp_${quote.symbol}`).text('00:00:00').appendTo(row)
        $('<td>').addClass('volume').attr('id', `volume_${quote.symbol}`).text(1).appendTo(row)
        $('<td>').addClass('open price').attr('id', `open_${quote.symbol}`).appendTo(row)
        $('<td>').addClass('low price').attr('id', `low_${quote.symbol}`).appendTo(row)
        $('<td>').addClass('high price').attr('id', `high_${quote.symbol}`).appendTo(row)

        row.appendTo(table)
    })

    startQuoteFeed()
})