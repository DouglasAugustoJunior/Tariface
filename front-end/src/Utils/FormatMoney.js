const FormatMoney = (valor) => {
    let saldoTsc = valor.toString().replace(".", ",");
    if(saldoTsc.substr(-2).indexOf(",") !== -1) {
        saldoTsc = saldoTsc + "0";
    } else if(saldoTsc.length === 1 || saldoTsc.indexOf(",") === -1){
        saldoTsc = `${saldoTsc},0`;
    }
    return saldoTsc;
}

export default FormatMoney;