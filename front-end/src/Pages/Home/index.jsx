import React, {useState, useEffect, useRef} from 'react';
import {FiUser, FiCreditCard, FiEdit, FiLogOut} from 'react-icons/fi';
import {useHistory} from 'react-router-dom';
import FormatMask from '../../Utils/FormatMask';
import { format } from 'date-fns';
import api from '../../api';
import { Table, Dropdown, Modal, Form, ControlLabel, FormControl, Button, InputPicker, DatePicker, Grid, Row, Col, Input} from 'rsuite';
import MultiDropzone from '../../components/MultiDropzone';
import SimpleDropzone from '../../components/SimpleDropzone';
import Card from '../../components/Card';
import './styles.css';

export default function Home() {
    const history = useHistory();
    const id = sessionStorage.getItem('id');
    const { Column, HeaderCell, Cell } = Table;
    const [modalCartao, setModalCartao] = useState(false);
    const [modalEditar, setModalEditar] = useState(false);
    const [foto, setFoto] = useState();
    const [nome, setNome] = useState();
    const [cpf, setCpf] = useState();
    const [senha, setSenha] = useState();
    const [confSenha, setConfSenha] = useState();
    const [email, setEmail] = useState();
    const [endereco, setEndereco] = useState();
    const [numeroCasa, setNumeroCasa] = useState();
    const [cep, setCep] = useState();
    const [complemento, setComplemento] = useState();
    const [municipio, setMunicipio] = useState();
    const [idMunicipio, setIdMunicipio] = useState();
    const [uf, setUf] = useState();
    const [idUf, setIdUf] = useState();
    const [saldo, setSaldo] = useState();
    const [titularCartao, setTitularCartao] = useState();
    const [numeroCartao, setNumeroCartao] = useState();
    const [validadeCartao, setValidadeCartao] = useState();
    const [csvCartao, setCsvCartao] = useState();
    const [cartoes, setCartoes] = useState([]);
    const [historico, setHistorico] = useState([]);
    const ref = useRef(saldo);
    
    api.defaults.headers.common['Authorization'] = `Bearer ${sessionStorage.getItem('token')}`;

    useEffect(() => {
        api.get(`usuario/pegaUsuarioPorID?idUsuario=${id}`)
        .then(response => {
            console.log(response.data);
            const getNome = response.data.nome;
            const getSaldo = response.data.saldo;
            const getCartoes = response.data.cartoes;
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
                    data: format(new Date(tsc.dataCriacao), 'dd/MM/yy'),
                    transacao: `Credito de R$: ${tsc.valor} adicionado`
                }
            });

            response.data.imagens.forEach(foto => {
                if(foto.perfil) {
                    const getFoto = foto.url;
                    setFoto(getFoto)
                }
            });

            setNome(getNome);
            setCpf(getCpf);
            setEmail(getEmail);
            setEndereco(getLogradouro);
            setNumeroCasa(getNumero);
            setComplemento(getComplemento);
            setCep(getCep);
            setUf(getEstado);
            setMunicipio(getMunicipio);
            setNome(getNome);
            setSaldo(getSaldo);
            setCartoes(getCartoes);
            setHistorico(getHistorico);
        });
    }, []);

    useEffect(() => {
        if(idUf) {
            api.get(`municipio/filtroUF?idUf=${idUf}`)
            .then(response => {
                const getmunicipio = response.data;
                setMunicipio(getmunicipio);
            });
        }
    }, [idUf]);

    async function handleSubmitUser(event) {
        event.preventDefault();
        const enviarFoto = new FormData();
        enviarFoto.append('arquivo', foto);
        const data = {
            "nome": nome, 
            "cpf": cpf && cpf.replace(/[^\d]+/g, ""),
            "email": email,
            "senha": senha,
            "endereco": {
                "logradouro": endereco,
                "numero": numeroCasa,
                "cep": cep && cep.replace(/[^\d]+/g, ""),
                "municipioId": idMunicipio,
                "complemento": complemento,
            },
        }

        try {
            await api.post('usuario/cadastrarUsuario', data)
            .then(async (resp) => {
                const id = resp.request.response;
                await api.post(`/imagem/uploadImagemPerfil?idUsuario=${id}`, enviarFoto);
            });
        } catch (error) {
            alert(`${error}`);
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
                    closeModalCartao();
                });
            });
        } catch (error) {
            alert(`${error}`);
            closeModalCartao();
        }
    }

    function logoff() {
        sessionStorage.clear();
        history.push('/');
    }

    function closeModalCartao() {
        setModalCartao(false);
      }

    function openModalCartao() {
        setModalCartao(true);
    }

    function closeModalEditar() {
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
                   
                    <Dropdown icon={<img src={foto} alt="Imagem Usuario" className="img-usuario"/>} >
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
                            id={cartao.id}
                            ref={ref}
                        />
                    ))}
               </div>
               
                <div id="extrato-usuario">
                    <h2>Histórico</h2>
                    <Table height={550} width={500} data={historico}>
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
                                        <Input style={{ width: 490 }} value={nome} name="nome" type="text" onChange={value => setNome(value)}/>
                                    </Col>
                                </Row>

                                <Row className="show-row" >
                                    <Col xs={12} className="show-col">
                                        <ControlLabel>CPF:</ControlLabel>
                                        <Input style={{ width: 220 }} id="cpf" value={cpf} onKeyPress={() => FormatMask("###.###.###-##", "cpf")} maxLength="14" type="text" onChange={value => setCpf(value)}/>
                                    </Col>

                                    <Col xs={12} className="show-col">
                                        <ControlLabel>E-mail:</ControlLabel>
                                        <Input style={{ width: 220 }} name="email" value={email} type="email" onChange={value => setEmail(value)}/>
                                    </Col>
                                </Row>

                                <Row className="show-row" >
                                    <Col xs={12} className="show-col">
                                        <ControlLabel>Senha:</ControlLabel>
                                        <Input style={{ width: 220 }} name="senha" type="password" onChange={value => setSenha(value)}/>
                                    </Col>

                                    <Col xs={12} className="show-col">
                                        <ControlLabel>Confirmar Senha:</ControlLabel>
                                        <Input style={{ width: 220 }} name="confSenha" type="password" onChange={value => setConfSenha(value)}/>
                                    </Col>
                                </Row>
                            </Col>
                        </Row>

                        <Row className="show-row">
                            <Row className="show-row">
                                <Col xs={10} className="show-col">
                                    <ControlLabel>Logradouro:</ControlLabel>
                                    <Input name="endereco" type="text" value={endereco} onChange={value => setEndereco(value)}/>
                                </Col>

                                <Col xs={4} className="show-col">
                                    <ControlLabel>Numero:</ControlLabel>
                                    <Input style={{ width: 100 }} maxLength="8" value={numeroCasa} name="numero" type="text" onChange={value => setNumeroCasa(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <ControlLabel>Complemento:</ControlLabel>
                                    <Input type="text" style={{ width: 250 }} value={complemento} onChange={value => setComplemento(value)}/>
                                </Col>
                            </Row>

                            <Row className="show-row">
                                <Col xs={4} className="show-col">
                                    <ControlLabel>CEP:</ControlLabel>
                                    <Input type="text" style={{ width: 100 }} id="cep" value={cep} onKeyPress={() => FormatMask("#####-###", "cep")} maxLength="9" onChange={value => setCep(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <p>Estado:</p>
                                    <InputPicker data={uf} labelKey="nome" valueKey="id" type="text" placeholder="Estado" style={{ height: 35, width: 250 }} onSelect={value => setIdUf(value)}/>
                                </Col>

                                <Col xs={10} className="show-col">
                                    <p>Municipio:</p>
                                    <InputPicker data={municipio} labelKey="nome" valueKey="id" placeholder="Municipio" type="text" style={{ height: 35, width: 250 }} onSelect={value => setIdMunicipio(value)}/>
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
                            <FormControl name="num-cartao" type="text" onChange={value => setNumeroCartao(value)}/>
                            
                            <div className="dataCartao">
                                <div>
                                    <ControlLabel>Validade:</ControlLabel>
                                    <DatePicker format="YYYY-MM" onChange={value => setValidadeCartao(format(new Date(value), 'MM/yyyy'))}/>
                                </div>

                                <div>
                                    <ControlLabel>CSV:</ControlLabel>
                                    <Input type="text" style={{ width: 100 }} onChange={value => setCsvCartao(value)}/>
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

            <Modal show={false} onHide={closeModalCartao}>
                <Modal.Header closeButton={false} >
                    <Modal.Title>Fotos para Reconhecimento Facial</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>
                        Para utilizar a funcionalidade de reconhecimento facial, você deve enviar pelo
                        menos 5 fotos de boa qualidade onde esteja sozinho.
                    </p>
                    <MultiDropzone/>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleAddCard} appearance="primary">Enviar</Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
}