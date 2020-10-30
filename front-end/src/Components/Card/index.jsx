import React, {useState} from 'react';
import { useHistory } from 'react-router-dom';
import FormatMask from "../../Utils/FormatMask";
import { format } from 'date-fns';
import api from '../../api';
import { DatePicker, Modal, Button, Form, FormGroup, FormControl, ControlLabel, Input } from 'rsuite';
import './styles.css';

export default function Card(props) {
    const history = useHistory();
    const [modalSaldo, setModalSaldo] = useState(false);
    const [modalEditar, setModalEditar] = useState(false);
    const [saldo, setSaldo] = useState(props.saldo);
    const [titularCartao, setTitularCartao] = useState();
    const [numeroCartao, setNumeroCartao] = useState();
    const [validadeCartao, setValidadeCartao] = useState();
    const [csvCartao, setCsvCartao] = useState();
    const id = sessionStorage.getItem('id');
    api.defaults.headers.common['Authorization'] = `Bearer ${sessionStorage.getItem('token')}`;

    async function handleSubmitCard(event) {
        event.preventDefault();
        const data = {
            "id": props.id,
            "titular": titularCartao ?  titularCartao : props.titular,
            "numero": numeroCartao ? numeroCartao : props.numero,
            "validade": validadeCartao ? validadeCartao : props.validade,
            "csv": csvCartao ? csvCartao : props.csv,
            "idUsuario": id
        }
        
        try {
            api.put('cartao/atualizar', data)
            closeModalEditar();
        } catch (error) {
            alert(`${error}`)
        }
    }

    async function handleAddSaldo(event) {
        event.preventDefault();
        try {
            api.post(`usuario/adicionaSaldo?idUsuario=${id}&valor=${saldo}`)
            .then(() => history.go(0));
        } catch (error) {
            alert(`${error}`)
        }
    }

    async function excluirCartao(event) {
        event.preventDefault();
        const data = {
            params: {
                "id": props.id
            }
        }

        try {
            api.delete('/cartao/excluir', data)
            .then(() => history.go(0));
        } catch (error) {
            alert(`${error}`);
            closeModalEditar();
        }
    }

    function closeModalSaldo() {
        setModalSaldo(false);
      }

    function openModalSaldo() {
        setModalSaldo(true)
    }

    function closeModalEditar() {
        setModalEditar(false);
      }

    function openModalEditar() {
        setModalEditar(true);
    }

    return (
        <div className="card-cartao">
            <span className="titular-cartao">{props.titular}</span>
            <span className="number-cartao">{props.numero}</span>

            <p>Validade</p>
            <DatePicker placeholder={props.validade} style={{ width: 150 }} disabled/>

            <div className="buttons-card">
                <Button onClick={openModalSaldo} className="novo-cartao" >Add Saldo</Button>
                <Button onClick={openModalEditar} className="editar-cartao" >Editar</Button>
            </div>

            <Modal size="xs" show={modalSaldo} onHide={closeModalSaldo} className="modal-add-saldo">
                <Modal.Header>
                    <Modal.Title>Adicionar Saldo</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <FormGroup>
                            <ControlLabel>Saldo Atual R$: {props.saldo}</ControlLabel>

                            <ControlLabel>Valor á ser adicionado</ControlLabel>
                            <Input id="add-valor" style={{ width: 200 }} type="text" placeholder="R$:" onChange={value => setSaldo(value)}/>
                        </FormGroup>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleAddSaldo} appearance="primary">Adicionar</Button>
                    <Button onClick={closeModalSaldo} >Cancel</Button>
                </Modal.Footer>
            </Modal>

            <Modal size="xs" show={modalEditar} onHide={closeModalEditar} >
                <Modal.Header>
                    <Modal.Title>Editar Cartão</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form>
                        <div className="modal-add-cartao">
                            <ControlLabel>Titular do Cartão:</ControlLabel>
                            <FormControl placeholder={props.titular} name="donoCartao" type="text" onChange={value => setTitularCartao(value)}/>
                            
                            <ControlLabel>Numero do Cartão:</ControlLabel>
                            <FormControl placeholder={props.numero} id="num-cartao" onKeyPress={() => FormatMask("#### #### #### ####", "num-cartao")} maxLength="19" type="text" onChange={value => setNumeroCartao(value)}/>
                            
                            <div className="dataCartao">
                                <div>
                                    <ControlLabel>Validade:</ControlLabel>
                                    <DatePicker placeholder={props.validade} format="YYYY-MM" onChange={value => setValidadeCartao(format(new Date(value), 'MM/yyyy'))}/>
                                </div>

                                <div>
                                    <ControlLabel>CSV:</ControlLabel>
                                    <Input placeholder={props.csv} type="text" style={{ width: 100 }} onChange={value => setCsvCartao(value)}/>
                                </div>
                            </div>
                        </div>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleSubmitCard} appearance="primary">Salvar</Button>
                    <Button onClick={closeModalEditar} >Cancelar</Button>
                    <Button onClick={excluirCartao} color="red">Excluir</Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
}