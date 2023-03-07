import { Button, Form, Input, Popconfirm, Table, Space } from 'antd';
import React, { useContext, useEffect, useRef, useState } from 'react';
import {GetDatosTareas, EliminarTarea, NuevaTarea, ModificarTarea} from  '../api/consumirapi'
import Cargando from '../cargando/cargar'
import './principal.css'

const EditableContext = React.createContext(null);

const EditableRow = ({ index, ...props }) => {
  const [form] = Form.useForm();
  return (
    <Form form={form} component={false}>
      <EditableContext.Provider value={form}>
        <tr {...props} />
      </EditableContext.Provider>
    </Form>
  );
};
const EditableCell = ({
    title,
    editable,
    children,
    dataIndex,
    record,
    handleSave,
    ...restProps
  }) => {
    const [editing, setEditing] = useState(false);
    const inputRef = useRef(null);
    const form = useContext(EditableContext);
    useEffect(() => {
      if (editing) {
        inputRef.current.focus();
      }
    }, [editing]);
    const toggleEdit = () => {
      setEditing(!editing);
      form.setFieldsValue({
        [dataIndex]: record[dataIndex],
      });
    };
    const save = async () => {
      try {
        const values = await form.validateFields();
        toggleEdit();
        //consumir api

        handleSave({
          ...record,
          ...values,
        });
      } catch (errInfo) {
        console.log('Error al guardar:', errInfo);
      }
    };

    let childNode = children;
    if (editable) {
      childNode = editing ? (
        <Form.Item
          style={{
            margin: 0,
          }}
          name={dataIndex}
          rules={[
            {
              required: true,
              message: `${title} es requerido.`,
            },
          ]}
        >
          <Input ref={inputRef} onPressEnter={save} onBlur={save} />
        </Form.Item>
      ) : (
        <div
          className="editable-cell-value-wrap"
          style={{
            paddingRight: 24,
          }}
          onClick={toggleEdit}
        >
          {children}
        </div>
      );
    }
    return <td {...restProps}>{childNode}</td>;
  };

const Principal= () =>{
      
    const [cargando, setCargando] = useState(false)
    const [consumiendoApi, setConsumiendoApi] = useState(false)
    const [respuestaApi, setRespuestaApi] = useState(null)
    const [count, setCount] = useState(0);   
    const [filtroAccion, setFiltroAccion]= useState([]);
    const components = {
        body: {
          row: EditableRow,
          cell: EditableCell,
        },
    };       

    const columnsDefecto = [
        {
          title: 'Detalle/Acción',
          dataIndex: 'accion',
          filters:filtroAccion,
          filterMode: 'tree',
          editable: true,
          filterSearch: true,
          onFilter: (text, record) => record.key===text,
          sorter: (a, b) => a.accion.length - b.accion.length,
          sortDirections: ['descend'],
          width: '30%',
        },
        {
          title: 'Estado',
          dataIndex: 'estado',
          sorter: (a, b) => a.completada - b.completada,
        },
        {
          title: 'Creado',
          dataIndex: 'fechaHoradecreacion',                 
          width: '40%',
        },    
        {
          title: 'modificado',
          dataIndex: 'fechahoradeactualizacion',          
          width: '40%',
        },
        {
            title: 'Operaciones',
            dataIndex: 'operation',
            render: (_, record: { key: React.Key }) =>
            respuestaApi.length >= 1 ? (
                <Space size="middle"> 
                <Popconfirm title="Esta seguro que quiere eliminar el registro?" onConfirm={() => handleDelete(record.key)}>
                    <a>Borrar</a>                    
                </Popconfirm>
                <Popconfirm title={"Esta seguro que quiere " + (record.completada?'No completada':'Completada') + " la tarea?"} onConfirm={() => handleCompletada(record.key)}>
                    <a>Act. {record.completada?'No completada':'Completada'}</a>
                    </Popconfirm>
                </Space>                                 
              ) : null,
        }
      ];

    const handleAdd = () => {
        const newData = {
          key: 0,
          accion: `Digite detelle acción ${count+1}`,
          completada: false,
          fechaHoradecreacion: new Date(),
          fechahoradeactualizacion:null
        };        
        setRespuestaApi([...respuestaApi, newData]);
        setCount(count + 1);
    };

    const handleSave = (row) => {
        const newData =respuestaApi;
        const index = newData.findIndex((item) => row.key === item.key);
        if (index<0 || row.key===0){
            //consumir api nueva tarea
            if(NuevaTarea(row)){                
                const item = newData[index];
                   newData.splice(index, 1, {
                   ...item,
                   ...row,
                   });
               setRespuestaApi(newData);
               setCount(count + 1);
               setConsumiendoApi(true);
           };
        }else{
            //consumir api guardar tarea
            if(ModificarTarea(row)){                
                const item = newData[index];
                   newData.splice(index, 1, {
                   ...item,
                   ...row,
                   });
               setRespuestaApi(newData);
               setCount(count + 1);
               setConsumiendoApi(true);
           };
        }
    };

    const handleDelete = (key) => {
        //consumir api        
        if(EliminarTarea(key)){
            //const newData = respuestaApi.filter((item) => item.key !== key);
            //setRespuestaApi(newData);            
            setCount(count - 1);
            setConsumiendoApi(true);
        };
        
    };

    const handleCompletada = (key) => {
        const newData =respuestaApi;
        const index = newData.findIndex((item) => key === item.key); 
        if (index>=0){
            let row =newData[index];
            row.completada=row.completada?false:true;
            if(ModificarTarea(row)){         
                setCount(count - 1);
                setConsumiendoApi(true);
            };
        }

       
        
    };


     // Relaliza la petición al back 
    const cosumirApiCargaTareas = async() => {        
        try 
        {            
            setCargando(true);
            const response= await GetDatosTareas();
            if(response!==null && !response.hasOwnProperty('mensaje')){                
                //recorrer la respuesta para armar los filtros de la columna acción 
                let filtro=[];
                response.forEach(function(registro) {
                    filtro.push({text:registro.accion, value:registro.key});
                    registro.estado=registro.completada ? 'Completada' : 'No completada';
                });
                //actualizar datos                     
                setCount(response.length+1);
                setConsumiendoApi(false);   
                setRespuestaApi(response); 
                setFiltroAccion(filtro);
            }     
            else{
                console.log(response.mensaje);
            //mostrar en un mensaje el error
            }                                          
        } catch (error) {
            console.log(error); 
            //mostrar en un mensaje el error
        }     
        finally{
            setCargando(false);            
        }   
    };

    const columns = columnsDefecto.map((col) => {
        if (!col.editable) {
          return col;
        }
        return {
          ...col,
          onCell: (record) => ({
            record,
            editable: col.editable,
            dataIndex: col.dataIndex,
            title: col.title,
            handleSave,
          }),
        };
      });

    useEffect(() => {
        if (!cargando){            
            cosumirApiCargaTareas();            
        }          
      
    }, [consumiendoApi]);


    if (!cargando){

        return (
            <div>
              <Button
                onClick={handleAdd}
                type="primary"
                style={{
                  marginBottom: 16,
                }}
              >
                Nueva Tarea
              </Button>
              <Table
                components={components}
                rowClassName={() => 'editable-row'}
                bordered
                dataSource={respuestaApi}
                columns={columns}
              />
            </div>
          );
          
    }
    else{
        return(<Cargando/>);
    }
     
 }

export default Principal;

