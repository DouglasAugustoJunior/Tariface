import React, { useState, useEffect } from 'react';
import api from '../../api';
import { format } from 'date-fns';
import { Table, } from 'rsuite';
import './styles.css';

export default function Report() {
    const { Column, HeaderCell, Cell } = Table;
    const id = sessionStorage.getItem('id');
    const [historico, setHistorico] = useState([]);
    api.defaults.headers.common['Authorization'] = `Bearer ${sessionStorage.getItem('token')}`;

    useEffect(() => {
        api.get(`usuario/pegaUsuarioPorID?idUsuario=${id}`)
        .then(response => {
            const getHistorico = response.data.historico.map(tsc => {
                return {
                    data: format(new Date(tsc.dataCriacao), 'dd/MM/yy hh:mm'),
                    transacao: `Efetuado ${tsc.tipo.nome} de R$: ${tsc.valor.toString().replace(".", ",")}`
                }
            });
            
            setHistorico(getHistorico.reverse());
        });
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div id="report-container">
            <div id="report-content">
                <h2>Relatório de Transações</h2>
                <Table data={historico} height={700} >
                    <Column flexGrow={20}  align="center">
                        <HeaderCell>Data</HeaderCell>
                        <Cell dataKey="data" className="negrito"/>
                    </Column>

                    <Column flexGrow={20} align="center" >
                        <HeaderCell>Transação</HeaderCell>
                        <Cell dataKey="transacao" />
                    </Column>

                </Table>
            </div>
        </div>
    )
}