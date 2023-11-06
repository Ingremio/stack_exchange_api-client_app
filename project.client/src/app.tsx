import { Route, Routes, useLocation, useNavigate } from "react-router-dom";
import "./app.css";
import { AppRoute } from "./appRoute";
import appRoutes from "./appRoutes";
import history from "./components/history";
import { Layout } from "./components/layout";

export default function App() {
    history.navigate = useNavigate();
    history.location = useLocation();

    return (
        <Layout>
            <Routes>
                {appRoutes.map((route, index) => {
                    const { element, ...rest } = route;

                    if (route.isPrivate) {
                        return <Route key={index} {...rest} element={
                            <AppRoute>
                                {element}
                            </AppRoute>
                        } />
                    } else {
                        return <Route key={index} {...rest} element={element} />
                    }
                })}
            </Routes>
        </Layout>
    );
}