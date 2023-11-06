import { Navigate } from "react-router-dom";
import history from "./components/history";
import { useAppSelector } from "./components/storage";

type AppRouteProps = {
    children: React.ReactNode
}

export function AppRoute(props: AppRouteProps) {
    const user = useAppSelector(x => x.userState.user);

    if (!user) {
        return <Navigate to="/login" state={{ from: history.location }} />
    }

    return props.children;
}