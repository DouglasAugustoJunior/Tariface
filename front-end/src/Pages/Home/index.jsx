/* eslint-disable no-unused-expressions */
import React, {useState, useEffect} from 'react';
import {FiCreditCard, FiEdit, FiLogOut} from 'react-icons/fi';
import {useHistory} from 'react-router-dom';
import FormatMask from '../../Utils/FormatMask';
import { format } from 'date-fns';
import api from '../../api';
import { Table, Dropdown, Modal, Form, ControlLabel,
 FormControl, Button, InputPicker, DatePicker, Grid,
 Row, Col, Input, Alert } from 'rsuite';
import MultiDropzone from '../../Components/MultiDropzone';
import SimpleDropzone from '../../Components/SimpleDropzone';
import Card from '../../Components/Card';
import User from '../../Assets/user.png';
import './styles.css';

export default function Home() {
    const { Column, HeaderCell, Cell } = Table;
    const [modalCartao, setModalCartao] = useState(false);
    const [usuario, setUsuario] = useState()
    const [modalEditar, setModalEditar] = useState(false);
    const [modalImagens, setModalImagens] = useState(false);
    const [foto, setFoto] = useState(User);
    const [nome, setNome] = useState();
    const [cpf, setCpf] = useState();
    const [senha, setSenha] = useState();
    const [confSenha, setConfSenha] = useState();
    const [email, setEmail] = useState();
    const [endereco, setEndereco] = useState();
    const [numeroCasa, setNumeroCasa] = useState();
    const [cep, setCep] = useState();
    const [complemento, setComplemento] = useState();
    const [municipio, setMunicipio] = useState([]);
    const [idMunicipio, setIdMunicipio] = useState();
    const [uf, setUf] = useState([]);
    const [idUf, setIdUf] = useState();
    const [saldo, setSaldo] = useState();
    const [titularCartao, setTitularCartao] = useState();
    const [numeroCartao, setNumeroCartao] = useState();
    const [validadeCartao, setValidadeCartao] = useState();
    const [csvCartao, setCsvCartao] = useState();
    const [cartoes, setCartoes] = useState([]);
    const [historico, setHistorico] = useState([]);
    const [atualiza, setAtualiza] = useState(false);
    const [errorVisible, setErrorVisible] = useState(false);
    const history = useHistory();
    const id = sessionStorage.getItem('id');
    const errorMessage = errorVisible ? 'Senhas diferentes !' : null;
    
    api.defaults.headers.common['Authorization'] = `Bearer ${sessionStorage.getItem('token')}`;

    useEffect(() => {
        api.get(`usuario/pegaUsuarioPorID?idUsuario=${id}`)
        .then(response => {
            const getNome = response.data.nome;
            const getSaldo = response.data.saldo;
            const getCartoes = response.data.cartoes;
            const getSenha = response.data.senha;
            const getCpf = response.data.cpf ;
            const getEmail = response.data.email ;
            const getLogradouro = response.data.endereco.logradouro ;
            const getNumero = response.data.endereco.numero ;
            const getComplemento = response.data.endereco.complemento ;
            const getCep = response.data.endereco.cep ;
            const getEstado = response.data.endereco.municipio.uf.nome ;
            const getMunicipio = response.data.endereco.municipio.nome ;
            const getHistorico = response.data.historico.map(tsc => {
                return {
                    data: format(new Date(tsc.dataCriacao), 'dd/MM/yy hh:mm'),
                    transacao: `Efetuado ${tsc.tipo.nome} de R$: ${tsc.valor.toString().replace(".", ",")}`
                }
            });
            response.data.imagens.length >= 8 ? setModalImagens(false) : setModalImagens(true);
            response.data.imagens.forEach(foto => {
                if(foto.perfil) {
                    const getFoto = foto.url;
                    setFoto(getFoto)
                }
            });
            
            setNome(getNome);
            setCpf(getCpf);
            setEmail(getEmail);
            setSenha(getSenha);
            setConfSenha(getSenha);
            setEndereco(getLogradouro);
            setNumeroCasa(getNumero);
            setComplemento(getComplemento);
            setCep(getCep);
            setUf(getEstado);
            setMunicipio(getMunicipio);
            setNome(getNome);
            setSaldo(getSaldo);
            setCartoes(getCartoes);
            setHistorico(getHistorico.reverse());
            setUsuario(response.data);
        });
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [atualiza]);

    useEffect(() => {
        if(idUf !== undefined) {
            api.get(`municipio/filtroUF?idUf=${idUf}`)
            .then(response => {
                const getmunicipio = response.data;
                setMunicipio(getmunicipio);
            });
        }
    }, [idUf]);

    useEffect(() => {
        senha !== confSenha ? setErrorVisible(true) : setErrorVisible(false);
    }, [senha, confSenha]);

    async function handleSubmitUser(event) {
        event.preventDefault();
        const enviarFoto = new FormData();
        enviarFoto.append('arquivo', foto);

        const data = {
            "id": id,
            "grupoPessoaId": usuario.grupoPessoaId,
            "nome": document.getElementById("input_nome").value,
            "cpf": document.getElementById("cpf").value.replace(/[^\d]+/g, ""),
            "saldo": saldo,
            "email": document.getElementById("input_email").value,
            "senha": senha ? senha : document.getElementById("input_senha").value,
            "endereco":{
                "logradouro": document.getElementById("input_endereco").value,
                "numero": document.getElementById("input_numero").value,
                "cep": document.getElementById("cep").value.replace(/[^\d]+/g, ""),
                "municipioId": idMunicipio ? idMunicipio : usuario.endereco.municipioId,
                "complemento": document.getElementById("input_complemento").value
            }
        }

        try {
            await api.put('/usuario/atualizarUsuario', data)
            .then(async () => {
                await api.post(`/imagem/uploadImagemPerfil?idUsuario=${id}`, enviarFoto)
                Alert.success('Dados atualizado com sucesso');
                setAtualiza(!atualiza)
                closeModalEditar();
            });
        } catch (error) {
            Alert.error('Falha ao atualizar os dados');
        }
    }

    async function handleAddCard(event) {
        event.preventDefault();
        const data = {
            "numero": numeroCartao && numeroCartao.replace(/[^\d]+/g, ""),
            "titular": titularCartao,
            "validade": validadeCartao,
            "csv": csvCartao,
            "idUsuario": parseInt(id)
        }

        try {
            await api.post('cartao/cadastrar', data)
            .then(async () => {
                api.get(`/cartao/${id}`)
                .then(response => {
                    const  getCartoes = response.data;
                    setCartoes(getCartoes);
                    Alert.success('Cartão adicionado com sucesso.')
                    closeModalCartao();
                });
            });
        } catch (error) {
            Alert.error('Falha ao adicionar o cartão.')
            closeModalCartao();
        }
    }

    function logoff() {
        sessionStorage.clear();
        history.push('/');
    }

    async function updateSaldo() {
        await api.get(`usuario/pegaUsuarioPorID?idUsuario=${id}`)
        .then((response) => {
            const getHistorico = response.data.historico.map(tsc => {
                return {
                    data: format(new Date(tsc.dataCriacao), 'dd/MM/yy'),
                    transacao: `Efetuado ${tsc.tipo.nome} de R$: ${tsc.valor}`
                }
            });
            setHistorico(getHistorico.reverse());
            setSaldo(response.data.saldo);
        })
    }

    async function updateCartao() {
        await api.get(`usuario/pegaUsuarioPorID?idUsuario=${id}`)
        .then((response) => {
            setCartoes(response.data.cartoes);
        })
    }

    function closeModalCartao() { setModalCartao(false) }

    function openModalCartao() { setModalCartao(true) }

    function closeModalEditar() {
        setAtualiza(!atualiza);
        setModalEditar(false);
    }

    function openModalEditar() {
        setModalEditar(true);
        api.get('uf')
        .then(response => {
            const getUf = response.data;
            setUf(getUf);
        });
    }

    return (
        <div id="home-container">
           <header id="header-home">
               <h2>TariFace</h2>
               <div className="info-usuario">
                   <div className="textos-usuario">
                        <span className="nome-usuario"> {nome} </span>
                        <span className="saldo-usuario"> Saldo R$: {saldo} </span>
                   </div>
                   
                    <Dropdown icon={<img src={foto} alt="" className="img-usuario"/>} >
                        <Dropdown.Item onSelect={openModalEditar}><FiEdit/> Editar Perfil</Dropdown.Item>
                        <Dropdown.Item onSelect={openModalCartao}><FiCreditCard/> Add Cartão</Dropdown.Item>
                        <Dropdown.Item onSelect={logoff}><FiLogOut/> Sair</Dropdown.Item>
                    </Dropdown>
               </div>
           </header>

           <section id="home-content">
               <div id="group-cartoes">
                    { cartoes.map(cartao => (
                        <Card key={`cartao-${cartao.id}`}
                            titular={cartao.titular}
                            numero={cartao.numero}
                            validade={cartao.validade}
                            csv={cartao.csv}
                            updateCartao={updateCartao}
                            updateSaldo={updateSaldo}
                            id={cartao.id}
                        />
                    ))}
               </div>
               
                <div id="extrato-usuario">
                    <h2>Histórico</h2>
                    <Table height={550} width={500} data={historico} >
                        <Column width={250} align="center" fixed>
                            <HeaderCell>Data</HeaderCell>
                            <Cell dataKey="data" />
                        </Column>

                        <Column width={250} align="center" fixed>
                            <HeaderCell>Transação</HeaderCell>
                            <Cell dataKey="transacao" />
                        </Column>
                    </Table>
                </div>
           </section>

           <Modal size="md" show={modalEditar} onHide={closeModalEditar} dialogClassName="modal-editar-perfil">
                <Modal.Header>
                    <Modal.Title>Editar Perfil</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <Grid id="modal-edital-perfil" fluid>
                        <Row className="show-row">
                            <Col xs={6} className="show-col field-photo">
                                <SimpleDropzone onFileUpload={setFoto}/>
                                <p>Foto de perfil</p>
                            </Col>

                            <Col xs={18} className="show-col">
                                <Row className="show-row">
                                    <Col xs={24} className="show-col">
                                        <p>Nome Completo:</p>
                                        <Input style={{ width: 490 }} value={nome} id="input_nome" type="text" onChange={value => setNome(value)}/>
                                    </Col>
                                </Row>

                                <Row className="show-row" >
                                    <Col xs={12} className="show-col">
                                        <ControlLabel>CPF:</ControlLabel>
                                        <Input style={{ width: 220 }} id="cpf" value={cpf} onKeyPress={() => FormatMask("###.###.###-##", "cpf")} maxLength="14" type="text" onChange={value => setCpf(value)}/>
                                    </Col>

                                    <Col xs={12} className="show-col">
                                        <ControlLabel>E-mail:</ControlLabel>
                                        <Input style={{ width: 220 }} id="input_email" value={email} type="email" onChange={value => setEmail(value)}/>
                                    </Col>
                                </Row>

                                <Row className="show-row" >
                                    <Col xs={12} className="show-col">
                                        <ControlLabel>Senha:</ControlLabel>
                                        <Input style={{ width: 220 }} id="input_senha" type="password" onChange={value => setSenha(value)}/>
                                    </Col>

                                    <Col xs={12} className="show-col">
                                        <ControlLabel>Confirmar Senha:</ControlLabel>
                                        <FormControl style={{ width: 220 }} errorMessage={errorMessage} id="input_confSenha" type="password" onChange={value => setConfSenha(value)}/>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>

                        <Row className="show-row">
                            <Row className="show-row">
                                <Col xs={10} className="show-col">
                                    <ControlLabel>Logradouro:</ControlLabel>
                                    <Input type="text" id="input_endereco" value={endereco} onChange={value => setEndereco(value)}/>
                                </Col>

                                <Col xs={4} className="show-col">
                                    <ControlLabel>Numero:</ControlLabel>
                                    <Input style={{ width: 100 }} maxLength="8" value={numeroCasa} id="input_numero" type="text" onChange={value => setNumeroCasa(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <ControlLabel>Complemento:</ControlLabel>
                                    <Input type="text" style={{ width: 250 }} id="input_complemento" value={complemento} onChange={value => setComplemento(value)}/>
                                </Col>
                            </Row>

                            <Row className="show-row">
                                <Col xs={4} className="show-col">
                                    <ControlLabel>CEP:</ControlLabel>
                                    <Input type="text" style={{ width: 100 }} id="cep" value={cep} onKeyPress={() => FormatMask("#####-###", "cep")} maxLength="9" onChange={value => setCep(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <p>Estado:</p>
                                    <InputPicker data={uf} labelKey="nome" id="input_estado" valueKey="id" type="text" placeholder="Estado" style={{ height: 35, width: 250 }} onSelect={value => setIdUf(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <p>Municipio:</p>
                                    <InputPicker disabled={idUf === undefined} data={municipio} labelKey="nome" id="input_municipio" valueKey="id" placeholder="Municipio" type="text" style={{ height: 35, width: 250 }} onSelect={value => setIdMunicipio(value)}/>
                                </Col>
                            </Row>
                        </Row>
                        </Grid>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleSubmitUser} appearance="primary">Ok</Button>
                    <Button onClick={closeModalEditar} appearance="subtle">Cancelar</Button>
                </Modal.Footer>
            </Modal>

            <Modal size="xs" show={modalCartao} onHide={closeModalCartao} >
                <Modal.Header>
                    <Modal.Title>Adicionar Cartão</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <div className="modal-add-cartao">
                            <ControlLabel>Titular do Cartão:</ControlLabel>
                            <FormControl name="donoCartao" type="text" onChange={value => setTitularCartao(value)}/>
                            
                            <ControlLabel>Numero do Cartão:</ControlLabel>
                            <FormControl name="num-cartao" id="num-cartao" type="text" onKeyPress={() => FormatMask("#### #### #### ####", "num-cartao")} maxLength="19" onChange={value => setNumeroCartao(value)}/>
                            
                            <div className="dataCartao">
                                <div>
                                    <ControlLabel>Validade:</ControlLabel>
                                    <DatePicker format="YYYY-MM" onChange={value => setValidadeCartao(format(new Date(value), 'MM/yyyy'))}/>
                                </div>

                                <div>
                                    <ControlLabel>CSV:</ControlLabel>
                                    <Input type="text" style={{ width: 100 }} maxLength="3" onChange={value => setCsvCartao(value)}/>
                                </div>
                            </div>
                        </div>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleAddCard} appearance="primary">Salvar</Button>
                    <Button onClick={closeModalCartao} appearance="subtle">Cancelar</Button>
                </Modal.Footer>
            </Modal>

            <Modal show={modalImagens} onHide={() => !modalImagens}>
                <Modal.Header closeButton={false} >
                    <Modal.Title>Fotos para Reconhecimento Facial</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>
                        Para utilizar a funcionalidade de reconhecimento facial, você deve enviar pelo
                        menos 8 fotos de boa qualidade onde esteja sozinho para o sistema.
                    </p>
                    <MultiDropzone />
                </Modal.Body>
            </Modal>
        </div>
    )
}