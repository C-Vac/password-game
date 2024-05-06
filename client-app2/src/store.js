import { createStore } from "redux";
import gameReducer from "./reducers"; // Assuming your reducer is in reducers.js

const store = createStore(gameReducer);

export default store;
