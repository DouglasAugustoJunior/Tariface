/* eslint-disable jsx-a11y/anchor-is-valid */
import React, {useState} from 'react';
import { FiTrash2 } from 'react-icons/fi'
import FormatMask from "../../Utils/FormatMask";
import { format } from 'date-fns';
import api from '../../api';
import { DatePicker, Modal, Button, Form,
 FormGroup, FormControl, ControlLabel, Input, Alert } from 'rsuite';
import './styles.css';

export default function Card(props) {
    const [modalSaldo, setModalSaldo] = useState(false);
    const [modalEditar, setModalEditar] = useState(false);
    const [saldo, setSaldo] = useState(props.saldo);
    const [titularCartao, setTitularCartao] = useState(props.titular);
    const [numeroCartao, setNumeroCartao] = useState(props.numero);
    const [validadeCartao, setValidadeCartao] = useState(props.validade);
    const [csvCartao, setCsvCartao] = useState(props.csv);
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
            .then(() => {
                props.updateCartao();
                Alert.success(`Cartão atualizado com sucesso`);
                closeModalEditar();
            })
        } catch (error) {
            Alert.error('Falha ao atualizar cartão!!!');
        }
    }

    async function handleAddSaldo(event) {
        event.preventDefault();
        try {
            if(saldo !== undefined) {
                api.post(`usuario/adicionaSaldo?idUsuario=${id}&valor=${saldo.replace(".", "").replace(",", ".")}`)
                .then(() => {
                    props.updateSaldo();
                    Alert.success(`Saldo adicionado com sucesso`);
                    closeModalSaldo();
                });
            }
        } catch (error) {
            Alert.error('Falha ao excluir cartão!!!');
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
            .then(() => {
                props.updateCartao();
                Alert.success(`Cartão excluido com sucesso`);
                closeModalEditar();
            });
        } catch (error) {
            Alert.error('Falha ao excluir cartão!!!');
        }
    }

    function closeModalSaldo() { setModalSaldo(false) }

    function openModalSaldo() { setModalSaldo(true) }

    function closeModalEditar() { setModalEditar(false) }

    function openModalEditar() { setModalEditar(true) }

    return (
        <div className="card-cartao">
            <span className="titular-cartao">{props.titular}</span>
            <span className="number-cartao">{props.numero}</span>

            <p>Validade</p>
            <DatePicker placeholder={format(new Date(props.validade), 'MM/yyyy')} style={{ width: 150 }} disabled/>

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
                            <ControlLabel>Valor a ser adicionado:</ControlLabel>
                            <Input id="add-valor" style={{ width: 200 }} type="text" maxLength="6" placeholder="R$:" onChange={value => setSaldo(value)}/>
                        </FormGroup>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleAddSaldo} appearance="primary">Adicionar</Button>
                    <Button onClick={closeModalSaldo} >Cancelar</Button>
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
                                    <DatePicker placeholder={format(new Date(props.validade), 'MM/yyyy')} format="YYYY-MM" onChange={value => setValidadeCartao(format(new Date(value), 'MM/yyyy'))}/>
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
                    <a onClick={excluirCartao} title="Excluir Cartão" className="btn-excluir-cartao"><FiTrash2 size={20}/></a>
                    <Button onClick={handleSubmitCard} appearance="primary">Salvar</Button>
                    <Button onClick={closeModalEditar} >Cancelar</Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
}