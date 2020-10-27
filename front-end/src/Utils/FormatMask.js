const FormatMask = (mascara, tag) => {
    const lgth = document.getElementById(tag).value.length;
    const saida = mascara.substring(0, 1);
    const texto = mascara.substring(lgth);
    
    if (texto.substring(0, 1) !== saida)
        document.getElementById(tag).value += texto.substring(0, 1);
}

export default FormatMask;