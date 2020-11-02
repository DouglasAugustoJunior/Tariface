import React, { useState, useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import FormatMask from '../../Utils/FormatMask';
import api from '../../api';
import { Form, Col, ControlLabel, Grid, Row,
 DatePicker, Button, Input, InputPicker, Alert,
 Loader, FormControl } from 'rsuite';
import { format } from 'date-fns';
import SimpleDropzone from '../../Components/SimpleDropzone';
import 'rsuite/dist/styles/rsuite-default.css';
import './styles.css';

export default function Register() {
    const [foto, setFoto] = useState();
    const [nome, setNome] = useState();
    const [cpf, setCpf] = useState();
    const [senha, setSenha] = useState();
    const [confSenha, setConfSenha] = useState();
    const [email, setEmail] = useState();
    const [endereco, setEndereco] = useState();
    const [numeroCasa, setNumeroCasa] = useState();
    const [complemento, setComplemento] = useState();
    const [cep, setCep] = useState();
    const [municipio, setMunicipio] = useState();
    const [idMunicipio, setIdMunicipio] = useState();
    const [uf, setUf] = useState();
    const [idUf, setIdUf] = useState();
    const [titularCartao, setTitularCartao] = useState();
    const [numeroCartao, setNumeroCartao] = useState();
    const [validadeCartao, setValidadeCartao] = useState();
    const [csvCartao, setCsvCartao] = useState();
    const [load, setLoad] = useState(false);
    const [errorVisible, setErrorVisible] = useState(false);
    const errorMessage = errorVisible ? 'Senhas diferentes !!!' : null;
    const history = useHistory();

    useEffect(() => {
        api.get('uf')
        .then(response => {
            const getUf = response.data;
            setUf(getUf);
        });
    }, []);

    useEffect(() => {
        senha !== confSenha ? setErrorVisible(true) : setErrorVisible(false);
    }, [senha, confSenha]);

    useEffect(() => {
        if(idUf){
            api.get(`municipio/filtroUF?idUf=${idUf}`)
            .then(response => {
                const getmunicipio = response.data;
                setMunicipio(getmunicipio);
            });
        }
    }, [idUf]);

    async function handleSubmit(event) {
        event.preventDefault();
        setLoad(true);
        const enviarFoto = new FormData();
        enviarFoto.append('arquivo', foto);
        const data = {
            "nome": nome, 
            "cpf": cpf && cpf.replace(/[^\d]+/g, ""),
            "saldo": 0,
            "email": email,
            "senha": senha,
            "endereco": {
                "logradouro": endereco,
                "numero": numeroCasa,
                "cep": cep && cep.replace(/[^\d]+/g, ""),
                "municipioId": idMunicipio,
                "complemento": complemento,
            },
            "cartoes": [{
                "numero": numeroCartao && numeroCartao.replace(/[^\d]+/g, ""),
                "titular": titularCartao,
                "validade": validadeCartao,
                "csv": csvCartao
            }]
        }

        try {
            console.log(enviarFoto)
            await api.post('usuario/cadastrarUsuario', data)
            .then(async (resp) => {
                const id = resp.request.response;
                await api.post(`/imagem/uploadImagemPerfil?idUsuario=${id}`, enviarFoto)
                .then(() => {
                    setLoad(false);
                    Alert.success(`Usuario com ID ${id} foi cadastrado com sucesso.`);
                    history.push('/');
                });
            });
        } catch (error) {
            setLoad(false);
            Alert.error('Falha ao cadastrar!!!')
        }
    }

    return (
        <div id="register-container">
            <div id="form-container">
                <h1>TariFace</h1>
                <Form id="form-register" onSubmit={handleSubmit} >
                    <Grid fluid>
                        <Row className="show-row">
                            <Col xs={6} className="show-col field-photo">
                                <SimpleDropzone onFileUpload={setFoto}/>
                                <p>Foto de perfil</p>
                            </Col>

                            <Col xs={18} className="show-col">
                                <Row className="show-row">
                                    <Col xs={24} className="show-col">
                                        <p>Nome Completo:</p>
                                        <Input style={{ width: 470 }} name="nome" type="text" onChange={value => setNome(value)}/>
                                    </Col>
                                </Row>

                                <Row className="show-row" gutter={16}>
                                    <Col xs={12} className="show-col">
                                        <ControlLabel>CPF:</ControlLabel>
                                        <Input style={{ width: 220 }} id="cpf" onKeyPress={() => FormatMask("###.###.###-##", "cpf")} maxLength="14" type="text" onChange={value => setCpf(value)}/>
                                    </Col>

                                    <Col xs={12} className="show-col">
                                        <ControlLabel>E-mail:</ControlLabel>
                                        <Input style={{ width: 220 }} name="email" type="email" onChange={value => setEmail(value)}/>
                                    </Col>
                                </Row>

                                <Row className="show-row" gutter={16}>
                                    <Col xs={12} className="show-col">
                                        <ControlLabel>Senha:</ControlLabel>
                                        <Input style={{ width: 220 }} name="senha" type="password" onChange={value => setSenha(value)}/>
                                    </Col>

                                    <Col xs={12} className="show-col">
                                        <ControlLabel>Confirmar Senha:</ControlLabel>
                                        <FormControl style={{ width: 220 }} errorMessage={errorMessage} name="confSenha" type="password" onChange={value => setConfSenha(value)}/>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>

                        <Row className="show-row">
                            <Row className="show-row">
                                <Col xs={10} className="show-col">
                                    <ControlLabel>Endereço:</ControlLabel>
                                    <Input type="text" onChange={value => setEndereco(value)}/>
                                </Col>

                                <Col xs={4} className="show-col">
                                    <ControlLabel>Numero:</ControlLabel>
                                    <Input style={{ width: 100 }} type="number" onChange={value => setNumeroCasa(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <ControlLabel>Complemento:</ControlLabel>
                                    <Input type="text" onChange={value => setComplemento(value)}/>
                                </Col>
                            </Row>

                            <Row className="show-row" gutter={25}>
                                <Col xs={4} className="show-col">
                                    <ControlLabel>CEP:</ControlLabel>
                                    <Input type="text" style={{ width: 100 }} id="cep" onKeyPress={() => FormatMask("#####-###", "cep")} maxLength="9" onChange={value => setCep(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <p>Estado:</p>
                                    <InputPicker data={uf} labelKey="nome" valueKey="id" type="text" placeholder="Estado" style={{ height: 35, width: 250 }} onSelect={value => setIdUf(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <p>Municipio:</p>
                                    <InputPicker disabled={idUf === undefined} data={municipio} labelKey="nome" valueKey="id" placeholder="Municipio" type="text" style={{ height: 35, width: 250 }} onSelect={value => setIdMunicipio(value)}/>
                                </Col>
                            </Row>
                        </Row>

                        <Row className="show-row">
                            <Row className="show-row">
                                <Col xs={12} className="show-col">
                                    <ControlLabel>Titular do Cartão:</ControlLabel>
                                    <Input name="donoCartao" type="text" onChange={value => setTitularCartao(value)}/>
                                </Col>

                                <Col xs={12} className="show-col">
                                    <p>Validade:</p>
                                    <DatePicker className="validade-cartao" format="MM/YY" style={{ width: 110 }} onChange={(value) => setValidadeCartao(format(new Date(value), 'MM/yyyy'))}/>
                                </Col>
                            </Row>

                            <Row className="show-row">
                                <Col xs={12} className="show-col">
                                    <ControlLabel>Numero do Cartão:</ControlLabel>
                                    <Input name="num-cartao" type="text" id="num-cartao" onKeyPress={() => FormatMask("#### #### #### ####", "num-cartao")} maxLength="19" onChange={value => setNumeroCartao(value)}/>
                                </Col>

                                <Col xs={12} className="show-col">
                                    <ControlLabel>CSV:</ControlLabel>
                                    <Input style={{ width: 110 }} maxLength="3" onChange={value => setCsvCartao(value)}/>
                                </Col>
                            </Row>
                        </Row>

                        <div className="group-form-butoes">
                            <Button type="submit" onClick={handleSubmit} className="bnt-confirm" >Confirmar</Button>
                            <Link to="/" className="cancel-register" >Cancelar</Link>
                        </div>
                    </Grid>
                </Form>
            </div>
            { load && <Loader backdrop size="lg" content="Aguarde..." vertical /> }
        </div>
    )
}