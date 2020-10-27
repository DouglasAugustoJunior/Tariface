import styled from "styled-components";

export const Container = styled.div`
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
`;

export const Content = styled.div`
  height: 250px;
  overflow: auto;
  width: 100%;
  max-width: 400px;
  background: #fff;
  border-radius: 4px;
  margin-top: 20px;
  padding: 10px;
  border: 1px #e5e5e5 solid;
  ::-webkit-scrollbar {
    width: 10px;
  }
  ::-webkit-scrollbar-thumb {
    background-color: #e5e5e5;
    border-radius: 4px;
  }
`;
