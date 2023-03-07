const urlBase = 'https://localhost:7256'
const errorResponse={id:0,
                    mensaje:'Respuesta no esperada del servicio'};

                    //Consumo de Api para obtener el listado completo de tareas
export const GetDatosTareas = async() => {
    const url = urlBase + '/api/Tareas' ;   
    const requestOptions = {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    };

    const retorno = await fetch(url, requestOptions)
        .catch(error => { 
            return {id:0,mensaje:error.message}});

    if (retorno !== undefined && retorno.status === 200) {                
        return await retorno.json();
        
    } else if (retorno !== undefined && retorno.status !== 200) {
        
    }
    return errorResponse
}

export const EliminarTarea = async(id) => {    
    const url = urlBase + '/api/Tareas/id=' + id;
    const requestOptions = {
        method: 'DELETE'        
    }

    const retorno = await fetch(url, requestOptions)
                        .catch(error => { 
                return {id:0,mensaje:error.message}});

    if (retorno !== undefined && retorno.status === 200) {
        return true
    } else if (retorno !== undefined && retorno.status !== 200) {
        let datos = retorno.json()
        datos.then(d => {
            let textoError = ''
            if (d.errors !== undefined) {
                let objetoError = Object.values(d.errors)
                for (let i = 0; i < objetoError.length; i++) {
                    textoError = `${textoError} ${i+1}- ${objetoError[i]}   `
                }
            } else {
                textoError = d.mensaje
            }            
        })
    }
    return false
}

export const NuevaTarea = async(datos) => {
    const url = urlBase + '/api/Tareas' ;   
    const requestOptions = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(datos)
    }

    const retorno = await fetch(url, requestOptions)
        .catch(error => { 
            return {id:0,mensaje:error.message}});

    if (retorno !== undefined && retorno.status === 200) {                
        return await retorno.json();
        
    } else if (retorno !== undefined && retorno.status !== 200) {
        
    }
    return errorResponse
}

export const ModificarTarea = async(datos) => {
    const url = urlBase + '/api/Tareas' ;   
    const requestOptions = {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(datos)
    }

    const retorno = await fetch(url, requestOptions)
        .catch(error => { 
            return {id:0,mensaje:error.message}});

    if (retorno !== undefined && retorno.status === 200) {                
        return await retorno.json();
        
    } else if (retorno !== undefined && retorno.status !== 200) {
        
    }
    return errorResponse
}